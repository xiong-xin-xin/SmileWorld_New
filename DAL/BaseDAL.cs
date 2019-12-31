using Dapper;
using DB;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
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
        public IDatabase GetDatabase() => _database;

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
        public async Task<IEnumerable<T>> GetPageListAsync<T>(Pagination pagination) where T : class, new()
        {
            if (pagination.sidx == null)
            {
                pagination.sidx = "id";
                pagination.sord = "desc";
            }
            string countSql = $"select count(1) from ({pagination.sql}) t";
            string pagesql = $@"SELECT TOP {pagination.pageSize} *
                                    FROM(SELECT ROW_NUMBER() OVER(ORDER BY {pagination.sidx} {pagination.sord}) AS RowNumber, *FROM ({pagination.sql}) t) as A
                                    WHERE RowNumber > {pagination.pageSize} * {pagination.pageIndex - 1}";
            return await _database.UseDbConnectionAsync(async t =>
             {
                 pagination.records = await t.ExecuteScalarAsync<int>(countSql, pagination.where);
                 return await t.QueryAsync<T>(pagesql, pagination.where);
             });
        }


        public async Task<List<T>> GetAllListAsync<T>(object where = null) where T : class, new()
        {
            T t = new T();
            string tableName = GetTableName<T>();
            string sql = $"select * from {tableName}";
            if (where != null)
            {
                sql += " where 1=1 ";
                foreach (PropertyInfo propertyInfo in where.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType.IsArray || propertyInfo.PropertyType.IsGenericType)
                    {
                        sql += $" and {propertyInfo.Name} in @{propertyInfo.Name}";
                    }
                    else
                    {
                        sql += $" and {propertyInfo.Name} = @{propertyInfo.Name}";
                    }
                }
            }
            var data = await _database.UseDbConnectionAsync(o => o.QueryAsync<T>(sql, where));
            return data.ToList();
        }


        /// <summary>
        /// 查询单记录
        /// </summary>
        public Task<T> QueryFirstAsync<T>(string vue, string key = "Id") where T : class, new()
        {
            string tableName = GetTableName<T>();

            string sql = $"select * from {tableName} where @{key} = @{key}";

            return _database.UseDbConnectionAsync(t => t.QueryFirstAsync<T>(sql, new { Id = vue }));
        }
        /// <summary>
        /// 删除
        /// </summary>
        public Task<int> DeleteAsync<T>(string vue, IDbTransaction tran = null) where T : class, new()
        {
            string tableName = GetTableName<T>();

            string sql = $"delete from {tableName} where Id in @Id";
            if (tran != null)
            {
                return tran.Connection.ExecuteAsync(sql, new { vulueof = vue.Split(',') }, tran);
            }
            return _database.UseDbConnectionAsync(t => t.ExecuteAsync(sql, new { Id = vue.Split(',') }));
        }
        public Task<int> DeleteAsync<T>(string key, string vue, IDbTransaction tran = null) where T : class, new()
        {
            string tableName = GetTableName<T>();

            string sql = $"delete from {tableName} where {key} = @{key}";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(key, vue);
            if (tran != null)
            {
                return tran.Connection.ExecuteAsync(sql, parameters, tran);
            }
            return _database.UseDbConnectionAsync(t => t.ExecuteAsync(sql, parameters));
        }

        /// <summary>
        /// 增加
        /// </summary>
        public Task<int> AddAsync<T>(T[] model, IDbTransaction tran = null) where T : class, new()
        {
            string tableName = GetTableName<T>();
            List<string> col = new List<string>();
            foreach (var item in model.First().GetType().GetProperties())
            {
                if (item.GetMethod.IsVirtual || item.PropertyType.IsGenericType) continue;
                col.Add(item.Name);
            }
            string a = string.Join(",", col.Select(p => p));
            string b = string.Join(",", col.Select(p => "@" + p));
            string sql = $"insert into {tableName}({a}) values({b})";
            if (tran != null)
            {
                return tran.Connection.ExecuteAsync(sql, model, tran);
            }
            return _database.UseDbConnectionAsync(t => t.ExecuteAsync(sql, model));
        }
        /// <summary>
        /// 增加
        /// </summary>
        public Task<int> AddAsync<T>(T model, IDbTransaction tran = null) where T : class, new()
        {
            string tableName = GetTableName<T>();
            List<string> col = new List<string>();
            foreach (var item in model.GetType().GetProperties())
            {
                if (item.GetMethod.IsVirtual || item.PropertyType.IsGenericType) continue;
                col.Add(item.Name);
            }
            string a = string.Join(",", col.Select(p => p));
            string b = string.Join(",", col.Select(p => "@" + p));
            string sql = $"insert into {tableName}({a}) values({b})";
            if (tran != null)
            {
                return tran.Connection.ExecuteAsync(sql, model, tran);
            }
            return _database.UseDbConnectionAsync(t => t.ExecuteAsync(sql, model));
        }

        public Task<int> EditAsync<T>(T model, string[] ignoreColumns = default(string[]), IDbTransaction tran = null) where T : class, new()
        {
            return EditAsync(model, null, ignoreColumns, tran);
        }

        /// <summary>
        /// 修改
        /// </summary>
        public Task<int> EditAsync<T>(T model, string[] updateColumns, string[] ignoreColumns, IDbTransaction tran = null) where T : class, new()
        {
            string tableName = GetTableName<T>();
            List<string> col = new List<string>();
            foreach (var item in model.GetType().GetProperties())
            {
                if (item.GetMethod.IsVirtual || item.PropertyType.IsGenericType) continue;
                if (item.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)) continue;
                if (ignoreColumns.Contains(item.Name, StringComparer.OrdinalIgnoreCase)) continue;
                if (updateColumns != null)
                {
                    if (updateColumns.Contains(item.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        col.Add(item.Name);
                    }
                }
                else
                {
                    col.Add(item.Name);
                }
            }
            string sql = $"update a set {string.Join(",", col.Select(c => $"a.{c}=@{c}"))} from {tableName} a where a.Id=@Id";
            if (tran != null)
            {
                return tran.Connection.ExecuteAsync(sql, model, tran);
            }
            return _database.UseDbConnectionAsync(t => t.ExecuteAsync(sql, model));
        }


    }
}
