using Dapper;
using DapperProject.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DapperProject.Connections
{
    /// <summary>
    ///     Dapper Only -> EF Core Removed
    /// </summary>
    public class AppReadDbConnection : IAppReadDbConnection, IDisposable
    {
        protected IDbConnection connection;
        /// <summary>
        ///     Dapper Only as reading doesn't generally need to share 
        ///     contexts with ef core so using a simple SQL Connection does the job.
        /// </summary>
        /// <param name="configuration"></param>
        public AppReadDbConnection(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction: transaction, commandType: commandType)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction: transaction, commandType: commandType);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(sql, param, transaction, commandType: commandType);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }
        }

    }

    /// <summary>
    ///     Dapper Read Connection 
    /// </summary>
    public class AppReadTransactionDbConnection : AppReadDbConnection, IAppReadTransaction, IDisposable
    {
        private IDbTransaction transaction;

        public AppReadTransactionDbConnection(IConfiguration configuration) : base(configuration)
        { }

        public void SetTransaction() {
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        public IDbTransaction GetTransaction() => transaction;

        public void Commit() {
            transaction.Commit();
            transaction.Dispose();
        }
    }
}
