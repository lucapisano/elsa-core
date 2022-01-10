using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Elsa.Persistence.EntityFramework.DocumentDb
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the context to use DocumentDb 
        /// </summary>
        public static DbContextOptionsBuilder UseDocumentDb(this DbContextOptionsBuilder builder, string connectionString, string dbName, Action<CosmosDbContextOptionsBuilder>? options = null) =>
            builder.UseCosmos(connectionString, dbName, options);
    }
}