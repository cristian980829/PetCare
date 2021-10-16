using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PetCare.Api.Data.Entities
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Detail> Details { get; set; }

        public DbSet<ClinicalHistory> ClinicalHistories { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<PetPhoto> PetPhotos { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<PetType> PetTypes { get; set; }

        public DbSet<Procedure> Procedures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pet>().HasIndex(x => x.Id).IsUnique();

            modelBuilder.Entity<Race>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<PetType>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<DocumentType>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<Procedure>().HasIndex(x => x.Description).IsUnique();
        }
    
    }
}
