using EF.Learning.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF.Learning.Storage
{
    public class TestDbContext : DbContext
    {
        public DbSet<DataLog> DataLogs { get; set; }
        public DbSet<Entity1> Entity1s { get; set; }
        public DbSet<Entity2> Entity2s { get; set; }


        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.Entity<Entity1>(entitiy =>
            {
                entitiy.HasKey(e => e.Entity1Id);
            });

            modelBuilder.Entity<Entity2>(entitiy =>
            {
                entitiy.HasKey(e => e.Entity2Id);
            });

            modelBuilder.Entity<DataLog>(entitiy =>
            {
                entitiy.HasKey(e => new { e.MainId, e.PartitionId });
                entitiy.HasOne(dl => dl.Entity1).WithMany(e1 => e1.DataLogs).HasForeignKey(e => e.Entity1Id);
                entitiy.HasOne(dl => dl.Entity2).WithMany(e2 => e2.DataLogs).HasForeignKey(e => e.Entity2Id);
                entitiy.HasOne(dl => dl.Parent).WithMany(e2 => e2.Children).HasForeignKey(e => new { e.ParentMainId, e.ParentPartitionId });
            });
        }
    }
}
