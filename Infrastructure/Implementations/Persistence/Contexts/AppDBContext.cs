using Domain.Entities.Expenditure;
using Domain.Entities.Ownership;
using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Persistence.Contexts;
public sealed class AppDBContext(DbContextOptions<AppDBContext> options)
    : DBContext(options)
{
    // Your DbSets go here
    public new DbSet<ApplicationRole> Roles { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillDetail> BillDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
      
        modelBuilder.Entity<ApplicationRole>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(256);

        modelBuilder.Entity<Bill>()
                    .HasOne(x=>x.PaidBy)
                    .WithMany()
                    .HasForeignKey(x=>x.PaidById)
                    .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Bill>()
                    .HasOne(x=>x.Organization)
                    .WithMany()
                    .HasForeignKey(x=>x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<BillDetail>()
                    .HasOne(x=>x.SharedWith)
                    .WithMany()
                    .HasForeignKey(x=>x.SharedWithId)
                    .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<BillDetail>()
                    .HasOne(x=>x.Bill)
                    .WithMany()
                    .HasForeignKey(x=>x.BillId)
                    .OnDelete(DeleteBehavior.Restrict);

        #region Indexing
        modelBuilder.Entity<ApplicationRole>()
            .HasIndex(x => x.Name);

        // ApplicationUser Indexes
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(x => x.Email);

        // Bill Indexes
        modelBuilder.Entity<Bill>()
            .HasIndex(x => x.PaidById)
            .IsUnique(false);

        modelBuilder.Entity<Bill>()
            .HasIndex(x => x.OrganizationId)
            .IsUnique(false);

        // BillDetail Indexes
        modelBuilder.Entity<BillDetail>()
            .HasIndex(x => x.SharedWithId)
            .IsUnique(false);

        modelBuilder.Entity<BillDetail>()
            .HasIndex(x => x.BillId)
            .IsUnique(false);

        #endregion Indexing

    }
}
