using System;
using System.Collections.Generic;
using System.Text;
using Exico.HF.DbAccess.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Exico.HF.DbAccess.Db
{
    public class ExicoHfDbContext : DbContext
    {

        public DbSet<HfUserJob> HfUserJob { get; set; }
        public ExicoHfDbContext(DbContextOptions<ExicoHfDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}