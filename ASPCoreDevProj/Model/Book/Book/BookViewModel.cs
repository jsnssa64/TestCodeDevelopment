using ASPCoreDevProj.Model.BookDTO.Author;
using ASPCoreDevProj.Model.BookDTO.Author.Interface;
using ASPCoreDevProj.Model.BookDTO.Book.Interface;
using ASPCoreDevProj.Model.BookDTO.Genre;
using ASPCoreDevProj.Model.BookDTO.Genre.Interface;
using System.Collections.Generic;

namespace ASPCoreDevProj.Model.BookDTO.Book
{
    public class BasicBook : IBasicBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfPublication { get; set; }
    }

    public class BookWithGenreAndAuthor : BasicBook, IBookWithGenreAndAuthor
    {
        public IList<AuthorBasic> Authors { get; set; }
        public IList<GenreBasic> Genres { get; set; }
    }
}
