using System;
using System.Collections.Generic;
using Me.One.Core.Contract.Business;
using Me.One.Core.Contract.Repository;

namespace Me.One.Core.Business
{
    public class BaseBusiness<T> : IBaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> _baseRepository;

        public BaseBusiness(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public virtual void BatchDelete(IEnumerable<T> entities, bool saveChanges = false)
        {
            _baseRepository.BatchDelete(entities, saveChanges);
        }

        public virtual void BatchInsert(IEnumerable<T> entities, bool saveChanges = false)
        {
            _baseRepository.BatchInsert(entities, saveChanges);
        }

        public virtual void BatchInsertOrUpdate(IEnumerable<T> entities, bool saveChanges = false)
        {
            _baseRepository.BatchInsertOrUpdate(entities, saveChanges);
        }

        public virtual void BatchMarkDelete(IEnumerable<T> entities, bool saveChanges = false)
        {
            _baseRepository.BatchMarkDelete(entities, saveChanges);
        }

        public virtual void BatchUpdate(IEnumerable<T> entities, bool saveChanges = false)
        {
            _baseRepository.BatchUpdate(entities, saveChanges);
        }

        public virtual void Delete(string id, bool saveChanges = false)
        {
            _baseRepository.Delete(id, saveChanges);
        }

        public T GetById(Guid id)
        {
            return _baseRepository.GetById(id);
        }

        public T GetById(string id)
        {
            return _baseRepository.GetById(id);
        }

        public IEnumerable<T> GetByIds(params string[] ids)
        {
            return _baseRepository.GetByIds(ids);
        }

        public virtual void Insert(T entity, bool saveChanges = false)
        {
            _baseRepository.Insert(entity, saveChanges);
        }

        public virtual void InsertOrUpdate(T entity, bool saveChanges = false)
        {
            _baseRepository.InsertOrUpdate(entity, saveChanges);
        }

        public virtual void MarkDelete(string id, bool saveChanges = false)
        {
            _baseRepository.MarkDelete(id, saveChanges);
        }

        public virtual void Update(T entity, bool saveChanges = false)
        {
            _baseRepository.Update(entity, saveChanges);
        }
    }
}