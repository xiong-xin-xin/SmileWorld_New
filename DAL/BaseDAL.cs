using Dapper;
using DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseDao : IBaseDao
    {
        protected readonly IDatabase _database;
        public BaseDao(IDatabase database)
        {
            _database = database;
        }
        public string GetTableName<T>() where T : class, new()
        {
            T t = new T();
            var objAttrs = t.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (objAttrs != null && objAttrs.Count() > 0)
            {
                var table = objAttrs.First() as TableAttribute;
                return table.Name;
            }
            return t.GetType().Name;
        }

        public async Task<List<T>> GetAllListAsync<T>(object where = null) where T : class, new()
        {
            T t = new T();
            string sql = "";
            var objAttrs = t.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (objAttrs != null && objAttrs.Count() > 0)
            {
                var table = objAttrs.First() as TableAttribute;
                sql = $"select * from {table.Name}";
                if (where != null)
                {
                    sql += " where 1=1 ";
                    foreach (PropertyInfo propertyInfo in where.GetType().GetProperties())
                    {
                        if (propertyInfo.GetType() == typeof(string[]))
                        {
                            sql += $" and {propertyInfo.Name} in @{propertyInfo.Name}";
                        }
                        else
                        {
                            sql += $" and {propertyInfo.Name} = @{propertyInfo.Name}";
                        }
                        
                    }
                }
            }
            var data = await _database.UseDbConnectionAsync(o => o.QueryAsync<T>(sql, where));
            return data.ToList();
        }
    }
}
