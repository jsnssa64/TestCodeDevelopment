using ASPCoreDevProj.Model.BookDTO.Genre.Interface;

namespace ASPCoreDevProj.Model.BookDTO.Genre
{
    public class GenreBasic : IGenreBasic
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class GenreWithDescription : GenreBasic, IGenreWithDescription
    {
        public string Description { get; set; }
    }
}
