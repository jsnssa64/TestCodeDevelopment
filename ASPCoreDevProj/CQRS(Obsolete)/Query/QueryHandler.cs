using ASPCoreDevProj.Data;
using DapperProject.Context;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Obsolete.Query
{
    public interface IQuery { }

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> Retrieve(TQuery query);
    }

    //  <summary>Abstract class to inherit and build query handler</summary>
    //  <typeparam name="TQuery"> Query must be of IQuery </typeparam>
    //	<typeparam name="TResult"> TResult can be of any time for example IEnumerable<Book> or just Book </typeparam>
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        protected IApplicationDbContext _context { get; set; }
        protected ILogger _Logger { get; set; }
        public QueryHandler(IApplicationDbContext context, ILogger Logger)
        {
            _context = context;
            _Logger = Logger;
        }

        //  <summary> User will use to execute Handle(...) and other safety measures</summary>
        //	<param name="Query"> Query To be passed into the handler </param>
        //	<returns> Result of TResult Type </returns>
        public async Task<TResult> Retrieve(TQuery query) {
            TResult result;

            try
            {
                result = await HandleAsync(query);
            }
            catch(Exception ex)
            {
                throw new Exception("" + ex);
            }
            return result;
        }

        //  <summary> Handler is to be inherited by the user to get Query results</summary>
        protected abstract Task<TResult> HandleAsync(TQuery query);
    }

}
