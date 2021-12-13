using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class DataContext : DbContext
    {
        //le constructeur
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {
                
        }
        //Pour que le Id s'incrémente automatiquent 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //to set a new Id automaticly
            modelBuilder.Entity<Owner>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<PortfolioItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");            
            modelBuilder.Entity<Contact>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Authontification>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<About>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            //To set a new data in Owner table.
            modelBuilder.Entity<Owner>().HasData(
                new Owner()
                {
                    Id = Guid.NewGuid(),
                    Avatar = "anwar.jpg",
                    FullName = "Anwar Ait el kosari",
                    Profile = ".Net core developer"
                });
        }
        //les tableaux
        public DbSet<Owner> Owner { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<Contact> Contact  { get; set; }
        public DbSet<Authontification> Authontification  { get; set; }
        public DbSet<About> About  { get; set; }

    }
}
