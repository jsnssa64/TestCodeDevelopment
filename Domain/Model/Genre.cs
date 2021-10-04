using System.Collections.Generic;

namespace Domain.Data.Model
{
    public interface IGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Genre : IGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Book> Books { get; set; }
        public IList<Books_Genres> GenresBooks { get; set; }
    }

    public class Books_Genres
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }
}
