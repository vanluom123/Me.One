using System;
using System.Collections.Generic;
using System.Linq;
using Me.One.Core.Contract.Repository;
using Microsoft.EntityFrameworkCore;

namespace Me.One.Core.Data
{
    public abstract class BaseRepository<T> :
        BaseReadRepository<T>,
        IBaseRepository<T>
        where T : BaseEntity, new()
    {
        private readonly DbContext _context;

        protected BaseRepository(DbContext context)
            : base(context)
        {
            _context = context;
        }

        private DbSet<T> DbSet => _context.Set<T>();

        private Action<bool> SaveChangeAction => saveChanges =>
        {
            if (!saveChanges)
                return;
            _context.SaveChanges();
        };

        public virtual void Insert(T entity, bool saveChanges = false)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            entity.LastUpdatedDateTime = DateTime.UtcNow;
            entity.RowVersion = 0;
            entity.Ticks = DateTime.UtcNow.Ticks;
            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();
            DbSet.Add(entity);
            SaveChangeAction(saveChanges);
        }

        public virtual void Update(T entity, bool saveChanges = false)
        {
            entity.LastUpdatedDateTime = DateTime.UtcNow;
            ++entity.RowVersion;
            entity.Ticks = DateTime.UtcNow.Ticks;
            UpdateInternal(entity, saveChanges);
        }

        public virtual void Delete(string id, bool saveChanges = false)
        {
            DbSet.Remove(GetById(id) ?? throw new System.Exception());
            SaveChangeAction(saveChanges);
        }

        public virtual void InsertOrUpdate(T entity, bool saveChanges = false)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                Insert(entity, saveChanges);
            else
                Update(entity, saveChanges);
        }

        public virtual void MarkDelete(string id, bool saveChanges = false)
        {
            var byId = GetById(id);
            if (byId == null)
                throw new System.Exception();
            byId.Deleted = true;
            Update(byId, saveChanges);
        }

        public virtual void BatchDelete(IEnumerable<T> entities, bool saveChanges = false)
        {
            DbSet.RemoveRange(entities);
            SaveChangeAction(saveChanges);
        }

        public virtual void BatchInsert(IEnumerable<T> entities, bool saveChanges = false)
        {
            var baseEntities = entities.ToList();
            foreach (var entity in baseEntities)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.LastUpdatedDateTime = DateTime.UtcNow;
                entity.RowVersion = 0;
                entity.Ticks = DateTime.UtcNow.Ticks;
                if (string.IsNullOrEmpty(entity.Id))
                    entity.Id = Guid.NewGuid().ToString();
            }

            DbSet.AddRange(baseEntities);
            SaveChangeAction(saveChanges);
        }

        public virtual void BatchInsertOrUpdate(IEnumerable<T> entities, bool saveChanges = false)
        {
            var baseEntities = entities.ToList();
            BatchInsert(baseEntities.Where((Func<T, bool>) (item => string.IsNullOrWhiteSpace(item.Id))), saveChanges);
            BatchUpdate(baseEntities.Where((Func<T, bool>) (item => !string.IsNullOrWhiteSpace(item.Id))), saveChanges);
        }

        public virtual void BatchMarkDelete(IEnumerable<T> entities, bool saveChanges = false)
        {
            var baseEntities = entities.ToList();
            foreach (var entity in baseEntities)
            {
                entity.Deleted = true;
                entity.LastUpdatedDateTime = DateTime.UtcNow;
                ++entity.RowVersion;
                entity.Ticks = DateTime.UtcNow.Ticks;
            }

            if (!DbSet.Local.Any((Func<T, bool>) (p => baseEntities.Any((Func<T, bool>) p.Equals))))
                DbSet.UpdateRange(baseEntities);
            SaveChangeAction(saveChanges);
        }

        public virtual void BatchUpdate(IEnumerable<T> entities, bool saveChanges = false)
        {
            var baseEntities = entities.ToList();
            foreach (var entity in baseEntities)
            {
                entity.LastUpdatedDateTime = DateTime.UtcNow;
                ++entity.RowVersion;
                entity.Ticks = DateTime.UtcNow.Ticks;
            }

            if (!DbSet.Local.Any((Func<T, bool>) (p => baseEntities.Any((Func<T, bool>) p.Equals))))
                DbSet.UpdateRange(baseEntities);
            SaveChangeAction(saveChanges);
        }

        public void Insert(T entity, bool autoSetAuditFields, bool saveChanges = false)
        {
            if (autoSetAuditFields)
            {
                Insert(entity, saveChanges);
            }
            else
            {
                if (string.IsNullOrEmpty(entity.Id))
                    entity.Id = Guid.NewGuid().ToString();
                DbSet.Add(entity);
                SaveChangeAction(saveChanges);
            }
        }

        public void Update(T entity, bool autoSetAuditFields, bool saveChanges = false)
        {
            if (autoSetAuditFields)
            {
                Update(entity, saveChanges);
            }
            else
            {
                ++entity.RowVersion;
                UpdateInternal(entity, saveChanges);
            }
        }

        public void MarkDelete(string id, bool autoSetAuditFields, bool saveChanges = false)
        {
            if (autoSetAuditFields)
            {
                MarkDelete(id, saveChanges);
            }
            else
            {
                var byId = GetById(id);
                if (byId == null)
                    throw new System.Exception();
                byId.Deleted = true;
                Update(byId, saveChanges, autoSetAuditFields);
            }
        }

        public void BatchInsert(IEnumerable<T> entities, bool autoSetAuditFields, bool saveChanges = false)
        {
            if (autoSetAuditFields)
            {
                BatchInsert(entities, saveChanges);
            }
            else
            {
                var baseEntities = entities.ToList();
                foreach (var obj in baseEntities.Where((Func<T, bool>) (x => string.IsNullOrEmpty(x.Id))))
                    obj.Id = Guid.NewGuid().ToString();
                DbSet.AddRange(baseEntities);
                SaveChangeAction(saveChanges);
            }
        }

        public void BatchUpdate(IEnumerable<T> entities, bool autoSetAuditFields, bool saveChanges = false)
        {
            if (autoSetAuditFields)
            {
                BatchUpdate(entities, saveChanges);
            }
            else
            {
                var baseEntities = entities.ToList();
                foreach (var entity in baseEntities)
                    ++entity.RowVersion;
                if (!DbSet.Local.Any((Func<T, bool>) (p => baseEntities.Any((Func<T, bool>) p.Equals))))
                    DbSet.UpdateRange(baseEntities);
                SaveChangeAction(saveChanges);
            }
        }

        public void InsertOrUpdate(T entity, bool autoSetAuditFields, bool saveChanges = false)
        {
            if (autoSetAuditFields)
                InsertOrUpdate(entity, saveChanges);
            else if (string.IsNullOrWhiteSpace(entity.Id))
                Insert(entity, saveChanges, autoSetAuditFields);
            else
                Update(entity, saveChanges, autoSetAuditFields);
        }

        public void BatchInsertOrUpdate(
            IEnumerable<T> entities,
            bool autoSetAuditFields,
            bool saveChanges = false)
        {
            var baseEntities = entities.ToList();
            BatchInsert(baseEntities.Where((Func<T, bool>) (item => string.IsNullOrWhiteSpace(item.Id))), saveChanges,
                autoSetAuditFields);
            BatchUpdate(baseEntities.Where((Func<T, bool>) (item => !string.IsNullOrWhiteSpace(item.Id))), saveChanges,
                autoSetAuditFields);
        }

        private void UpdateInternal(T entity, bool saveChanges = false)
        {
            var entityEntry = _context.Entry(entity);
            var entity1 = _context.Set<T>().Local.FirstOrDefault((Func<T, bool>) (x => x.Id == entity.Id));
            if (entity1 != null && entityEntry.State == EntityState.Detached)
                foreach (var property in _context.Entry(entity1).Properties)
                {
                    var propertyEntry = entityEntry.Property(property.Metadata.Name);
                    var currentValue = property.CurrentValue;
                    if ((currentValue != null ? currentValue.Equals(propertyEntry.CurrentValue) ? 1 :
                            0 :
                            property.CurrentValue == propertyEntry.CurrentValue ? 1 : 0) == 0 &&
                        !property.Metadata.IsPrimaryKey() && !property.Metadata.IsKey())
                    {
                        property.CurrentValue = propertyEntry.CurrentValue;
                        property.IsModified = true;
                    }
                }
            else
                _context.Update(entity);

            SaveChangeAction(saveChanges);
        }
    }
}