using ASPCoreDevProj.Model.BookDTO.Author;
using ASPCoreDevProj.Model.BookDTO.Author.Interface;
using ASPCoreDevProj.Model.BookDTO.Genre;
using ASPCoreDevProj.Model.BookDTO.Genre.Interface;
using System.Collections.Generic;

namespace ASPCoreDevProj.Model.BookDTO.Book.Interface
{
    public interface IBasicBook
    {
        int Id { get; }
        string Title { get; }
        int YearOfPublication { get; }
    }
    public interface IAuthors
    {
        IList<AuthorBasic> Authors { get; }
    }
    public interface IGenres
    {
        IList<GenreBasic> Genres { get; }
    }

    public interface IBooksAuthors : IBasicBook, IAuthors { }
    public interface IBooksGenres : IBasicBook, IGenres { }
    public interface IBookWithGenreAndAuthor : IBooksAuthors, IBooksGenres { }
}
