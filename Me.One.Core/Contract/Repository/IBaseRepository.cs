using System.Collections.Generic;

namespace Me.One.Core.Contract.Repository
{
    public interface IBaseRepository<T> : IBaseReadRepository<T> where T : class, new()
    {
        void Insert(T entity, bool saveChanges = false);

        void Insert(T entity, bool autoSetAuditFields, bool saveChanges = false);

        void Update(T entity, bool saveChanges = false);

        void Update(T entity, bool autoSetAuditFields, bool saveChanges = false);

        void Delete(string id, bool saveChanges = false);

        void MarkDelete(string id, bool saveChanges = false);

        void MarkDelete(string id, bool autoSetAuditFields, bool saveChanges = false);

        void BatchInsert(IEnumerable<T> entities, bool saveChanges = false);

        void BatchInsert(IEnumerable<T> entities, bool autoSetAuditFields, bool saveChanges = false);

        void BatchUpdate(IEnumerable<T> entities, bool saveChanges = false);

        void BatchUpdate(IEnumerable<T> entities, bool autoSetAuditFields, bool saveChanges = false);

        void BatchDelete(IEnumerable<T> entities, bool saveChanges = false);

        void BatchMarkDelete(IEnumerable<T> entities, bool saveChanges = false);

        void InsertOrUpdate(T entity, bool saveChanges = false);

        void InsertOrUpdate(T entity, bool autoSetAuditFields, bool saveChanges = false);

        void BatchInsertOrUpdate(IEnumerable<T> entities, bool saveChanges = false);

        void BatchInsertOrUpdate(IEnumerable<T> entities, bool autoSetAuditFields, bool saveChanges = false);
    }
}