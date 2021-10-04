using System.Collections.Generic;

namespace Domain.Data.Model
{
    public interface IAuthor
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    public class Author : IAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
        public IList<Authors_Books> AuthorsBooks { get; set; }
        public AuthorsDOB DOB { get; set; }
    }

    public class AuthorsDOB
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }

    public class Authors_Books
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int BookId { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
    }

}
