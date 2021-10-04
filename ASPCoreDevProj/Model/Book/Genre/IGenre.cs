using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Model.BookDTO.Genre.Interface
{
    public interface IGenreBasic
    {
        int? Id { get; set; }
        string Name { get; set; }
    }

    public interface IGenreDescription
    {
        string Description { get; set; }
    }

    public interface IGenreWithDescription : IGenreBasic, IGenreDescription { }
}
