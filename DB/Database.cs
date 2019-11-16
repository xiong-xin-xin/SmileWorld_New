using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Database : IDatabase
    {
        private readonly IDbConnection _dbConnection;

        public Database(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<TEntity> UseDbConnectionAsync<TEntity>(Func<IDbConnection, Task<TEntity>> func)
        {
            try
            {
                var result = await func(_dbConnection);
                return result;
            }
            catch (Exception e)
            {
                
            }
            return default(TEntity);
        }
        public async Task<bool> UseDbTransactionAsync(Func<IDbConnection, IDbTransaction, Task> func)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            var b = false;
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    await func(_dbConnection, transaction);
                    transaction.Commit();
                    b = true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    
                }
            }
            return b;
        }

    }
}
