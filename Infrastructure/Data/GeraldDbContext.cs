
namespace Gerald.Infrastructure.Data
{
    public class GeraldDbContext : DbContext
    {
        public GeraldDbContext(DbContextOptions<GeraldDbContext> options) : base(options) { }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<AuditFinding> AuditFindings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vendor configuration
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(255);
                entity.Property(v => v.RegistrationNumber).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Status).HasMaxLength(50);
                entity.HasMany(v => v.Findings).WithOne(f => f.Vendor).HasForeignKey(f => f.VendorId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(v => v.RegistrationNumber).IsUnique();
            });

            // AuditFinding configuration
            modelBuilder.Entity<AuditFinding>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Severity).IsRequired().HasMaxLength(20);
                entity.Property(f => f.Status).IsRequired().HasMaxLength(50);
                entity.Property(f => f.Notes).IsRequired();
            });
        }
    }
}