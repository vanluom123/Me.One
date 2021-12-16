using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Me.One.Core.Contract.Repository;

namespace Me.One.Core.Data.Dapper
{
    public abstract class DapperBaseRepository<T> : IDapperRepository<T>, IDisposable
        where T : BaseEntity, new()
    {
        protected readonly DbConnection _connection;
        protected readonly string _tableName;

        protected DapperBaseRepository(DbConnection connection)
        {
            _connection = connection;
            _tableName = GetTableName();
        }

        public T GetById(Guid id)
        {
            return GetById(id.ToString());
        }

        public T GetById(string id)
        {
            return _connection.Get<T>(id);
        }

        public IEnumerable<T> GetByIds(params string[] ids)
        {
            return _connection.Query<T>("SELECT * FROM " + _tableName + " WHERE Id IN @Ids", new
            {
                Ids = ids
            });
        }

        public void Insert(T entity, bool saveChanges = false)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            entity.LastUpdatedDateTime = DateTime.UtcNow;
            entity.RowVersion = 0;
            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();
            _connection.Insert(entity);
        }

        public void Update(T entity, bool saveChanges = false)
        {
            entity.LastUpdatedDateTime = DateTime.UtcNow;
            ++entity.RowVersion;
            _connection.Update(entity);
        }

        public void Delete(string id, bool saveChanges = false)
        {
            _connection.Delete(GetById(id) ?? throw new System.Exception());
        }

        public void InsertOrUpdate(T entity, bool saveChanges = false)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                Insert(entity, saveChanges);
            else
                Update(entity, saveChanges);
        }

        public void MarkDelete(string id, bool saveChanges = false)
        {
            var byId = GetById(id);
            if (byId == null)
                throw new System.Exception();
            byId.Deleted = true;
            Update(byId, saveChanges);
        }

        public void BatchDelete(IEnumerable<T> entities, bool saveChanges = false)
        {
        }

        public void BatchInsert(IEnumerable<T> entities, bool saveChanges = false)
        {
            foreach (var entity in entities)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.LastUpdatedDateTime = DateTime.UtcNow;
                entity.RowVersion = 0;
                if (string.IsNullOrEmpty(entity.Id))
                    entity.Id = Guid.NewGuid().ToString();
                _connection.Insert(entity);
            }
        }

        public void BatchInsertOrUpdate(IEnumerable<T> entities, bool saveChanges = false)
        {
            BatchInsert(entities.Where((Func<T, bool>) (item => string.IsNullOrWhiteSpace(item.Id))), saveChanges);
            BatchUpdate(entities.Where((Func<T, bool>) (item => !string.IsNullOrWhiteSpace(item.Id))), saveChanges);
        }

        public void BatchMarkDelete(IEnumerable<T> entities, bool saveChanges = false)
        {
            foreach (var entity in entities)
            {
                entity.Deleted = true;
                entity.LastUpdatedDateTime = DateTime.UtcNow;
                ++entity.RowVersion;
                _connection.Update(entity);
            }
        }

        public void BatchUpdate(IEnumerable<T> entities, bool saveChanges = false)
        {
            foreach (var entity in entities)
            {
                entity.LastUpdatedDateTime = DateTime.UtcNow;
                ++entity.RowVersion;
                _connection.Update(entity);
            }
        }

        public abstract Task<IEnumerable<T>> ListAsync(int page = 1, int size = 50);

        public virtual async Task ExecuteAsync(string sql, object param, CommandType cmdType = CommandType.Text)
        {
            var num = await _connection.ExecuteAsync(sql, param, commandType: cmdType);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(string query, object param = null)
        {
            return await _connection.QueryAsync<T>(query, param);
        }

        public virtual void Dispose()
        {
        }

        private string GetTableName()
        {
            var obj = new T();
            var property = typeof(T).GetProperty("TypeName", BindingFlags.Instance | BindingFlags.Public);
            return property != null ? property.GetValue(obj, null) as string : typeof(T).Name;
        }
    }
}