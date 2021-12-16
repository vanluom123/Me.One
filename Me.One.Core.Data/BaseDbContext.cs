using Microsoft.EntityFrameworkCore;

namespace Me.One.Core.Data
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual void OnContextInit()
        {
        }
    }
}