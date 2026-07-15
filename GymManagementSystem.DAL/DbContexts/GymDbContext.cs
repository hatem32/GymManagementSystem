using GymManagementSystem.Configurations;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementSystem.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(EB =>
            {
                EB.Property(X => X.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50);

                EB.Property(X => X.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            });
        }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
       
    }
}
