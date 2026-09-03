using Microsoft.EntityFrameworkCore;
using optum_healthcare_system.Models.Entities;

namespace optum_healthcare_system.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        //represents the labresults table in the database
        public DbSet<LabResult> LabResults { get; set; }

        //Define the model configuration for the LabResult entity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabResult>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.PatientId).IsRequired();
                entity.Property(e => e.TestName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ResultValue).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(10);
                entity.Property(e => e.ObservedDate).IsRequired();

                entity.Property(e => e.CreatedOn).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.UpdatedOn);
                entity.Property(e => e.UpdatedBy);

                entity.HasIndex(e => e.PatientId);
                entity.HasIndex(e => e.ObservedDate);
            });
        }
    }
}
