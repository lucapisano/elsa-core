using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Elsa.Persistence.EntityFramework.Core.Extensions
{
    internal static class DocumentDbDatabaseFacadeExtensions
    {
        public static bool IsDocumentDb(this DatabaseFacade database) => database.ProviderName == "Microsoft.EntityFrameworkCore.Cosmos";
    }
}
