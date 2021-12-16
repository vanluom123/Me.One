using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Me.One.Core.Contract.Repository
{
    public interface IDapperRepository<T> where T : new()
    {
        T GetById(Guid id);

        T GetById(string id);

        IEnumerable<T> GetByIds(params string[] ids);

        void Insert(T entity, bool saveChanges = false);

        void Update(T entity, bool saveChanges = false);

        void Delete(string id, bool saveChanges = false);

        void MarkDelete(string id, bool saveChanges = false);

        void BatchInsert(IEnumerable<T> entities, bool saveChanges = false);

        void BatchUpdate(IEnumerable<T> entities, bool saveChanges = false);

        void BatchDelete(IEnumerable<T> entities, bool saveChanges = false);

        void BatchMarkDelete(IEnumerable<T> entities, bool saveChanges = false);

        void InsertOrUpdate(T entity, bool saveChanges = false);

        void BatchInsertOrUpdate(IEnumerable<T> entities, bool saveChanges = false);

        Task<IEnumerable<T>> ListAsync(int page = 1, int size = 50);

        Task<IEnumerable<T>> ListAsync(string query, object param = null);

        Task ExecuteAsync(string sql, object param, CommandType cmdType = CommandType.Text);
    }
}