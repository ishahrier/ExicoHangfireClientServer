using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Exico.HF.DbAccess.Db
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public abstract TContext CreateDbContext(string[] args);

        protected virtual string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        protected virtual string GetBasePath()
        {
            return Directory.GetCurrentDirectory();
        }

        protected string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetBasePath())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{GetEnvironmentName()}.json", true)
                .AddEnvironmentVariables();
            var config = builder.Build();
            var connstr = config.GetConnectionString(GetConnectionStringName());
            if (string.IsNullOrWhiteSpace(connstr))
            {
                throw new InvalidOperationException($"Could not find a connection string named '{GetConnectionStringName()}'.");
            }
            return connstr;
        }

        protected DbContextOptions<TContext> GetDbContextOptions()
        {
            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Using the following DB connection:");
            Console.ForegroundColor = ConsoleColor.Green;
            var connStringParts = connectionString.Split(';').Select(t => t.Split(new char[] { '=' }, 2)).ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
            connStringParts.Keys.ToList().ForEach(x => Console.WriteLine($"{x} : {connStringParts[x]}"));
            Console.ResetColor();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder = ConfigureOptionBuilder(optionsBuilder);
            var options = optionsBuilder.Options;
            return options;
        }
        protected virtual DbContextOptionsBuilder<TContext> ConfigureOptionBuilder(DbContextOptionsBuilder<TContext> optionBuilder)
        {
            return optionBuilder;
        }
        protected virtual string GetConnectionStringName()
        {
            return "HangFire";
        }
    }
}
