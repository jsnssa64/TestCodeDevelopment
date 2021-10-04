using ASPCoreDevProj.Data;
using DapperProject.Context;
using DapperProject.Interfaces;
using Domain.Data.Model;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQS.Data.BookQuery
{
    public class AddBookCommand : IRequest
    {
        public Book Book { get; set; }
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        public IAppWriteDbConnection _context { get; }
        public IDbTransaction transaction { get; }
        public AddBookCommandHandler(IAppWriteDbConnection context, ApplicationDbContext appContext)
        {
            _context = context;
            transaction = appContext.Database.CurrentTransaction.GetDbTransaction();
        }

        public async Task<Unit> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            string sql = "update Table set Title = @book.Title, YearOfPublication = @book.YearOfPublication where Id = @book.Id";
            await _context.ExecuteAsync(sql, request, transaction, CommandType.Text, cancellationToken);
            Console.WriteLine("The Update Book Command has been run.");
            return Unit.Value;
        }
    }
}
