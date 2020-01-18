using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ServerTest.Db
{
    class UsersDataContext : DbContext
    {
        public DbSet<User> Users { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=UsersDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User("admin", "admin"));
            modelBuilder.Entity<User>().HasData(new User("admin2", "admin"));
            modelBuilder.Entity<User>().HasData(new User("admin3", "admin"));
        }
    }
}
