using System.Collections.Generic;

namespace Domain.Data.Model
{
    public interface IBook
    {
        int Id { get; set; }
        string Title { get; set; }
        public int YearOfPublication { get; set; }
    }

    public interface IAuthors
    {
        ICollection<Author> Authors { get; set; }
        IList<Authors_Books> BooksAuthors { get; set; }
    }
    public interface IGenres
    {
        ICollection<Genre> Genres { get; set; }
        IList<Books_Genres> BooksGenres { get; set; }
    }

    public interface IDetailedBook : IBook, IAuthors, IGenres { } 

    //  EF Core Entity Object
    public class Book : IDetailedBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfPublication { get; set; }

        public ICollection<Author> Authors { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public IList<Authors_Books> BooksAuthors { get; set; }
        public IList<Books_Genres> BooksGenres { get; set; }
    }

}
