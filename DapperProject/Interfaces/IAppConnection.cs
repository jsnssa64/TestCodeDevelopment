using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DapperProject.Interfaces
{
    public interface IAppReadDbConnection
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text,  CancellationToken cancellationToken = default);
        Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text,  CancellationToken cancellationToken = default);
    }

    public interface IAppTransaction
    {
        //  Might not be necessary
        IDbTransaction GetTransaction();
        void SetTransaction();
    }

    //  
    public interface ITransactionCommit
    {
        void Commit();
    }

    //  Enable Ef Core SaveChangesAsync()
    public interface ICommitAsync
    {
        Task Complete(CancellationToken cancellationToken);
    }

    public interface IAppReadTransaction : IAppReadDbConnection, IAppTransaction, ITransactionCommit { }
    public interface IAppWriteTransaction : IAppWriteDbConnection, IAppTransaction, ICommitAsync { }

    public interface IAppWriteDbConnection : IAppReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
    }
}
