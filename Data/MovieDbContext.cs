using Microsoft.EntityFrameworkCore;
using MovieBooking.Models;

namespace MovieBooking.Data
{
    public class MovieDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-5PQ7GK2;Initial Catalog=MovieBooking;Integrated Security=True;TrustServerCertificate=True");
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Booking> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
