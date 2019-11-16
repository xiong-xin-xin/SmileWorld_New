using System;
using System.Data;
using System.Threading.Tasks;

namespace DB
{
    public interface IDatabase
    {
        Task<TEntity> UseDbConnectionAsync<TEntity>(Func<IDbConnection, Task<TEntity>> func);
        Task<bool> UseDbTransactionAsync(Func<IDbConnection, IDbTransaction, Task> func);
    }
}
