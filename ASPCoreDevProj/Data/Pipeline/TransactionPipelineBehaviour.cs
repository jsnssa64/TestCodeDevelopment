using CQS.Data.BookQuery;
using Dapper.Transaction;
using DapperProject.Context;
using DapperProject.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Data.Pipeline
{
    public interface ITransactionalRequest { }

    //  Transactional with save changes
    //  ITransactionalPipeline allows it to pick and choose which CQRS handlers you would like this pipeline to use
    public class TransactionPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactionalRequest
    {
        private readonly IApplicationDbContext context;
        private readonly IAppWriteTransaction contexttemp;

        public TransactionPipelineBehaviour(IApplicationDbContext context, IAppWriteTransaction contexttemp)
        {
            this.context = context;
            this.contexttemp = contexttemp;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            contexttemp.SetTransaction();
            //context.Connection.Open();
            //using (IDbTransaction connectionTransaction = context.Connection.BeginTransaction())
            //{
            // Register with EF Core
            //context.Database.UseTransaction(connectionTransaction as DbTransaction);
                try
                {
                    var response = await next();

                    await contexttemp.Commit(cancellationToken);
                    /*await context.SaveChangesAsync(cancellationToken);
                    connectionTransaction.Commit();*/
                    return response;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            //}
        }
    }
}
