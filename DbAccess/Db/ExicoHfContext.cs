using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Exico.Hf.DbAccess.Db
{


    public class ExicoHfDbContext : DbContext
    {
        public ExicoHfDbContext(DbContextOptions<ExicoHfDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }


}


