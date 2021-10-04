using ASPCoreDevProj.Model.BookDTO.Author.Interface;
using System;

namespace ASPCoreDevProj.Model.BookDTO.Author
{
    public class AuthorBasic : IAuthorBasic
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class AuthorWithDOB : AuthorBasic, IAuthorWithDOB
    {
        public DateTime DOB { get; set; }
    }
}
