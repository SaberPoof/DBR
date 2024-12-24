using DBRobot.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DBRobot.Data
{
    public class PgContext : DbContext
    {
        private static readonly string _schema = "mp_schema";
        public DbSet<Test> dbData { get; set; }

        public PgContext(DbContextOptions<PgContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_schema);

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("Test", _schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }
    }
}
