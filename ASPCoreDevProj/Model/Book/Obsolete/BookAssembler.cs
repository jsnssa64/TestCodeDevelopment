using ASPCoreDevProj.Model.BookDTO.Book;
using CQRS;
using Domain.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Model.Book
{
	public class FullBookAssembler : IAssembler<IBook, BasicBook>
	{
		public BasicBook Build(IBook dbInstanceResult) => new BasicBook()
		{
			Id = dbInstanceResult.Id,
			Title = dbInstanceResult.Title,
			YearOfPublication = dbInstanceResult.YearOfPublication
		};
	}
}
