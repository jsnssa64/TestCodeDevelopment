using AutoMapper;
using DapperProject.Context;
using DapperProject.Interfaces;
using Domain.Data.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASPCoreDevProj.Extension;
using ASPCoreDevProj.Data.Pipeline;
using System.Reflection;
using System.Text;

namespace CQS.Data.BookQuery
{
    public class UpdateBookCommand : IRequest, ITransactionalRequest
    {
        public Book book { get; set; }
    }

    public class UpdateBookUsingExistingDependenciesCommandHandler : IRequestHandler<UpdateBookCommand>
    {
        public IAppWriteDbConnection context { get; }
        public IApplicationDbContext appContext { get; }
        public IDbTransaction transaction { get; }
        public IMapper mapper { get; set; }
        public UpdateBookUsingExistingDependenciesCommandHandler(IAppWriteDbConnection context, IApplicationDbContext appContext, IMapper mapper)
        {
            this.context = context;
            transaction = appContext.Database.CurrentTransaction.GetDbTransaction();
            this.appContext = appContext;
            this.mapper = mapper;
        }

        public bool HasBeenUpdated<T>(T origin, T updated, string[] KeyList, out string[] UpdatedKeys)
        {
            var count = 0;
            Type TypeO = origin.GetType();
            Type TypeU = updated.GetType();

            UpdatedKeys = new string[KeyList.Length];
            //  Check Objects are the same
            if (TypeO == TypeU)
                throw new Exception("Object Type Must Be Equal");

            foreach (var Key in KeyList)
            {
                var PropO = TypeO.GetProperty(Key);
                object propOriginValue = PropO.GetValue(origin, null);

                var PropU = TypeU.GetProperty(Key);
                object propUpdatedValue = PropU.GetValue(updated, null);

                if (propOriginValue != propUpdatedValue)
                {
                    UpdatedKeys[count] = Key;
                    count++;
                }
            }

            if (count > 0)
                return true;
            return false;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            //  Get Book with Genre and Authors with just IDs -> EF Core
            Book dbAllBooksAndRef = await appContext.Books
                                                    .Include(bk => bk.BooksGenres)
                                                    .Include(bk => bk.BooksAuthors)
                                                        .Where(bg => bg.Id == request.book.Id)
                                                        .FirstOrDefaultAsync();

            //  Update Specific Values of Book
            string[] UpdatedKeys;
            if (HasBeenUpdated<Book>(dbAllBooksAndRef, request.book, new string[] { "Id", "Title", "YearOfPublication" }, out UpdatedKeys))
            {
                StringBuilder UpdateBase = new StringBuilder("Update Books set ", 1000);
                for(var i = 0; i < UpdatedKeys.Length; i++)
                {
                    UpdateBase.AppendJoin(UpdatedKeys[i], " = @book.", UpdatedKeys[i]);
                    if (UpdatedKeys.Length - 1 != i)
                        UpdateBase.Append(", ");
                }

                string sql = UpdateBase.Append(" where Id = @book.Id").ToString();

                var affected = await context.ExecuteAsync(sql, request, transaction, CommandType.Text, cancellationToken);
                if (affected == 0)
                    throw new DbUpdateException("Was Unable To Update Book: " + request.book.Title);
            }

            //  Books Existing Genres/Authors
            List<Genre> requestGenres = request.book.Genres.ToList();
            List<Author> requestAuthors = request.book.Authors.ToList();

            //  If failed to remove that means that records has been removed
            List<Books_Genres> deleteGenrelist = dbAllBooksAndRef.Genres
                                                        .Where(dbGenre => !requestGenres.Remove(dbGenre))
                                                        .Select(dbGenre => new Books_Genres()
                                                        {
                                                            BookId =    request.book.Id,
                                                            GenreId =   dbGenre.Id
                                                        }).ToList();

            appContext.BooksGenres.RemoveRange(deleteGenrelist);

            //  Add Existing not 
            appContext.BooksGenres.AddRange(requestGenres.Select(newGenre => new Books_Genres()
            {
                GenreId = newGenre.Id,
                BookId = request.book.Id
            }));

            await appContext.SaveChangesAsync(cancellationToken);

            //  If failed to remove that means that records has been removed
            List<Authors_Books> deleteAuthorlist = dbAllBooksAndRef.Authors
                                                        .Where(author => !requestAuthors.Remove(author))
                                                        .Select(author => new Authors_Books()
                                                        {
                                                            BookId = request.book.Id,
                                                            AuthorId = author.Id
                                                        }).ToList();

            appContext.AuthorsBooks.RemoveRange(deleteAuthorlist);

            appContext.AuthorsBooks.AddRange(requestAuthors.Select(newAuthor => new Authors_Books()
            {
                Author = newAuthor,
                BookId = request.book.Id
            }));


            await appContext.SaveChangesAsync(cancellationToken);

            Console.WriteLine("The Update Book Command has been run.");
            return Unit.Value;
        }
    }
}
