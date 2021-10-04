using ASPCoreDevProj.Data.Pipeline;
using ASPCoreDevProj.Model.BookDTO.Book;
using AutoMapper;
using DapperProject.Interfaces;
using Domain.Data.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQS.Data.BookQuery
{

    //  Filter for query Builder
    public class GetAllBookQuery : IRequest<ListOfBooks>, ITransactionalReadOnlyRequest
    {
        public string TitleMustContain { get; set; }
        public int? FromPublishDate { get; set; } = null;
        public int ToPublishedDate { get; set; } = DateTime.Now.Year;
    }

    public class ListOfBooks
    {
        public IEnumerable<BasicBook> Books { get; set; }
    }

    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, ListOfBooks> 

    {
        private readonly IAppReadTransaction _context;
        private readonly IMapper _mapper;
        public GetAllBookQueryHandler(IAppReadTransaction context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListOfBooks> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            StringBuilder sqlbuilder = new("SELECT b.Title, b.YearOfPublication FROM Books AS b WHERE b.YearOfPublication <= @ToPublishedDate ");
            if (request.FromPublishDate != null)
            {
                sqlbuilder.Append("AND b.YearOfPublication >= @FromPublishDate ");
            }
            
            if (!String.IsNullOrWhiteSpace(request.TitleMustContain))
            {
                sqlbuilder.Append("AND b.YearOfPublication LIKE '%@TitleMustContain%' ");
            }
            

            IEnumerable<Book> listOfBooks = await _context.QueryAsync<Book>(sqlbuilder.ToString(), request, _context.GetTransaction(), CommandType.Text, cancellationToken);

            ListOfBooks results = new ()
            {
                Books = _mapper.Map<IEnumerable<Book>, IEnumerable<BasicBook>>(listOfBooks)
            }; 
            return results;
        }

        /*  EF Core Example of Exression Tree   */
        /*private async Task<ListOfBooks> HandleEFCoreExample(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            ListOfBooks results = new ListOfBooks();
            var testter = typeof(Book);
            var parameterExpression = Expression.Parameter(typeof(Book), "Book");

            var YOfPubTo = Expression.Property(parameterExpression, "YearOfPublication");
            var ToPublishDate = Expression.Constant(request.ToPublishedDate);
            var TPDExp = Expression.LessThanOrEqual(YOfPubTo, ToPublishDate);

            if (request.FromPublishedDate != null)
            {
                var YOfPubFrom = Expression.Property(parameterExpression, "YearOfPublication");
                var FromPublishDate = Expression.Constant(request.FromPublishedDate);
                var FPDExp = Expression.GreaterThanOrEqual(YOfPubFrom, FromPublishDate);
                TPDExp = Expression.And(TPDExp, FPDExp);
            }

            if (!String.IsNullOrWhiteSpace(request.TitleMustContain))
            {
                var Title = Expression.Property(parameterExpression, "Title");
                var TValue = Expression.Constant(request.TitleMustContain);
                //  bks.Title.Contains(request.TitleMustContain)
                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsMethodExp = Expression.Call(Title, containsMethod, new Expression[] { TValue });
                TPDExp = Expression.And(TPDExp, containsMethodExp);
            }


            var lambda = Expression.Lambda<Func<Book, bool>>(TPDExp, parameterExpression);
            IQueryable<Book> getbooks = _context.Books.Where(lambda);

            results.Books = await getbooks.ProjectTo<BaseBook>(_mapper.ConfigurationProvider).ToListAsync();
            return results;
        }*/
    }
}
