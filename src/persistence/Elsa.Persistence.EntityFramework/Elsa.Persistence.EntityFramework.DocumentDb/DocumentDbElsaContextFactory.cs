using Elsa.Persistence.EntityFramework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;

namespace Elsa.Persistence.EntityFramework.DocumentDb
{
    public class DocumentDbElsaContextFactory : IDesignTimeDbContextFactory<ElsaContext>
    {
        public ElsaContext CreateDbContext(string connectionString, string dbName, Action<CosmosDbContextOptionsBuilder> options = null)
        {
            var builder = new DbContextOptionsBuilder<ElsaContext>();
            builder.UseCosmos(
                connectionString,
                dbName);
            var context = new ElsaContext(builder.Options);
            context.Database.EnsureCreated();
            return context;
        }
        public ElsaContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ElsaContext>();
            var connectionString = args.Any() ? args[0] : throw new InvalidOperationException("Please specify a connection string. E.g. dotnet ef database update -- \"Server=localhost;Port=3306;Database=elsa;User=root;Password=password\"");
            var dbName = args.Length >= 2 ? args[1] : null;
            return CreateDbContext(connectionString, dbName);
        }
    }
}