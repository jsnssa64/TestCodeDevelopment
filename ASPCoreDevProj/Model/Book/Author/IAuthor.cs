using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Model.BookDTO.Author.Interface
{
    public interface IAuthorBasic
    {
        int? Id { get; set; }
        string Name { get; set; }
    }
    public interface IDOB
    {
        DateTime DOB { get; set; }
    }

    public interface IAuthorWithDOB : IAuthorBasic, IDOB { }
}
