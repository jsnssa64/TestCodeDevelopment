using ASPCoreDevProj.Data;
using DapperProject.Context;
using DapperProject.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Data.BookQuery
{
    public class DeleteBookCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public DeleteBookCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, int>
    {
        public IAppWriteDbConnection _context { get; }
        public IDbTransaction transaction { get; }
        public ILogger _logger { get; }
        public DeleteBookCommandHandler(IAppWriteDbConnection context, ApplicationDbContext appContext)
        {
            _context = context;
            transaction = appContext.Database.CurrentTransaction.GetDbTransaction();
        }

        public async Task<int> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var sql = "DELETE FROM Products WHERE ProductId = @DeleteId";
            var result = await _context.ExecuteAsync(sql, new { DeleteId = command.Id }, transaction, CommandType.Text, cancellationToken);
            return result;
        }
    }
}
