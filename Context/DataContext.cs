using SeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SeyahatRehberi
{
    public class DataContext:DbContext
    {
        public DataContext() : base("DbConnection") {

        }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<BlogComment> BlogComment { get; set; }
        public DbSet<BlogLike> BlogLike { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}