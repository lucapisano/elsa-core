using Elsa.Attributes;
using Elsa.Persistence.EntityFramework.Core;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Persistence.EntityFramework.DocumentDb
{
    [Feature("DefaultPersistence:EntityFrameworkCore:DocumentDb")]
    public class Startup : EntityFrameworkCoreStartupBase
    {
        protected override string ProviderName => "DocumentDb";
        protected string DefaultDbName => "Elsa";
        protected override void Configure(DbContextOptionsBuilder options, string connectionString) => options.UseDocumentDb(connectionString, DefaultDbName);
    }
}