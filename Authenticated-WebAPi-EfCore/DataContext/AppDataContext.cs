using day2efcoredemo.Models;
using Microsoft.EntityFrameworkCore;

namespace day2efcoredemo.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "Dani",
                LastName = "Din",
                Password = "1234567",
                UserName = "user1"
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                FirstName = "Dana",
                LastName = "Din",
                Password = "1234567",
                UserName = "user2"
            });
        }
    }
}