using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Database : IDatabase,IDisposable
    {
        private readonly ILogger<Database> _log;
        private readonly IDbConnection _dbConnection;
        private bool _disposed;
        public Database(IDbConnection dbConnection,ILogger<Database> log)
        {
            _log = log;
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
                _log.LogError(e.Message,e);
                throw e;
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
                    _log.LogError(e.Message, e);
                    throw e;
                }
            }
            return b;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbConnection?.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
