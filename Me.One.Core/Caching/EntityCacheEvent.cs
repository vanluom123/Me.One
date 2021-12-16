namespace Me.One.Core.Caching
{
    public class EntityCacheEvent
    {
        public EntityCacheEvent(string entity, CacheEvent cacheevent)
        {
            Entity = entity;
            Event = cacheevent;
        }

        public string Entity { get; }

        public CacheEvent Event { get; }
    }
}