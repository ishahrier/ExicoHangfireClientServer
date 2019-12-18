using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Db
{
    public interface IGenerateDbContext
    {
        ExicoHfDbContext GenerateNewContext();
    }

    public class GenerateDbContext : IGenerateDbContext
    {
        public ExicoHfDbContext GenerateNewContext()
        {
            ExicoHfDbFactory factory = new ExicoHfDbFactory(false);
            return factory.CreateDbContext(null);
        }
    }
}
