using Postcode.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Postcode.Data
{
    public class postcodeContext : DbContext
    {
        public postcodeContext(DbContextOptions<postcodeContext> options)
          : base(options)
        {
        }

        public DbSet<PostcodeCoordinate> PostcodeCoordinates { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostcodeCoordinate>().ToTable("PostcodeCoordinate");
            modelBuilder.Entity<PostcodeCoordinate>().HasKey(e => e.Postcode);
            modelBuilder.Entity<PostcodeCoordinate>().Property(e => e.Postcode).HasMaxLength(10);

        }
    }
}
