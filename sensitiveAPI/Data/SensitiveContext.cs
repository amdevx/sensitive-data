using Microsoft.EntityFrameworkCore;
using sensitiveAPI.Entities;

namespace sensitiveAPI.Data
{
    public class SensitiveContext : DbContext
    {
        public SensitiveContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SensitiveDataEntity> SensitiveDataEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensitiveDataEntity>()
                .HasIndex(c => c.EncryptionKeyName);

            base.OnModelCreating(modelBuilder);
        }
    }
}