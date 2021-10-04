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
    public interface ITransactionalReadOnlyRequest { }

    //  Transaction without save changes
    public class ReadOnlyTransactionPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactionalReadOnlyRequest
    {
        private readonly IAppReadTransaction readOnlyContext;
        public ReadOnlyTransactionPipelineBehaviour(IAppReadTransaction readOnlyContext)
        {
            this.readOnlyContext = readOnlyContext;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            readOnlyContext.SetTransaction();
            try
            {
                var response = await next();
                readOnlyContext.Commit();
                return response;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
