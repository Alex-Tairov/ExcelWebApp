using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using ExcelWebApp.Db.Models;

namespace PersonDb.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

       
    }
}