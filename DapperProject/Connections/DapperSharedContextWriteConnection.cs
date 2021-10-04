using Dapper;
using DapperProject.Context;
using DapperProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DapperProject.Connections
{
    public class DapperSharedContextWriteConnection : IAppWriteDbConnection
    {
        protected readonly IApplicationDbContext context;

        /// <summary>
        ///     Sharing context means when you write to the same SQL connection 
        ///     as EF Core, any transactions can be rolled back from both dapper and EF Core
        /// </summary>
        /// <param name="context"></param>
        public DapperSharedContextWriteConnection(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///     The difference between read and write is here ... you can execute later the 
        ///    method later on unlike read which should use and throw away the connection 
        ///    ExecuteAsync -> No return values
        /// </summary>
        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await context.Connection.ExecuteAsync(sql, param, transaction, commandType: commandType);
        }
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return (await context.Connection.QueryAsync<T>(sql, param, transaction, commandType: commandType)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandType: commandType);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QuerySingleAsync<T>(sql, param, transaction, commandType: commandType);
        }
    }

    public class DapperSharedContextWriteTransactionConnection : DapperSharedContextWriteConnection, IAppWriteTransaction
    {
        private IDbTransaction transaction;
        public DapperSharedContextWriteTransactionConnection(IApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        ///     Used to retrieve existing transaction from the ef core context...
        ///     Not sure whether this is completely okay or whether there is some issues 
        ///     with using this transaction.
        /// </summary>
        /// <returns>Returns Transaction or null if doesn't exist</returns>
        public IDbTransaction GetTransaction() => context.Database.CurrentTransaction.GetDbTransaction();

        public async Task Complete(CancellationToken cancellationToken)
        {
            //  Possibly separate into its own function so you don't have to savechanges while commiting
            await context.SaveChangesAsync(cancellationToken);
            //  Commit and Dispose
            transaction.Commit();
            transaction.Dispose();
        }

        public void SetTransaction()
        {
            context.Connection.Open();
            transaction = context.Connection.BeginTransaction();
            //  Enable EF Core To Use transaction
            context.Database.UseTransaction(transaction as DbTransaction);
        }
    }
}
