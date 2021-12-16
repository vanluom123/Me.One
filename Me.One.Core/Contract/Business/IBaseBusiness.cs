using System;
using System.Collections.Generic;

namespace Me.One.Core.Contract.Business
{
    public interface IBaseBusiness<T> where T : BaseEntity, new()
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
    }
}