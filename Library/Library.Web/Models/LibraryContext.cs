using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using Library.Core.Models;

namespace Library.Web.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Books> Books { get; set; }
        public DbSet<Readers> Readers { get; set; }
        public DbSet<ReadersBooks> ReadersBooks {get;set;}
        public DbSet<Users> Users { get; set; }
    }
}