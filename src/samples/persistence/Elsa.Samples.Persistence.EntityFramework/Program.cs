using System;
using System.Threading.Tasks;
using Elsa.Persistence;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Persistence.EntityFramework.DocumentDb;

namespace Elsa.Samples.Persistence.EntityFramework
{
    class Program
    {
        private static async Task Main()
        {
            var connStr = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var dbName = "Elsa";
            // Create a service container with Elsa services.
            var services = new ServiceCollection()
                .AddElsa(options => options
                    // Configure Elsa to use the Entity Framework Core persistence provider using one of the three available providers 
                    .UseEntityFrameworkPersistence(ef =>
                    {
                        //ef.UseSqlite();
                        
                        //ef.UsePostgreSql("Server=127.0.0.1;Port=5432;Database=elsa;User Id=postgres;Password=password;");

                        //ef.UseMySql("Server=localhost;Port=3306;Database=elsa;User=root;Password=password;");

                        //ef.UseSqlServer("Server=localhost;Database=Elsa;Integrated Security=true");
                        
                        ef.UseDocumentDb(
                            connStr,
                            dbName);
                    }, false)
                    .AddConsoleActivities()
                    .AddWorkflow<HelloWorld>())
                .AddAutoMapperProfiles<Program>()
                .BuildServiceProvider();

            var ctx = new DocumentDbElsaContextFactory().CreateDbContext(connStr, dbName);
            ctx.Database.EnsureCreated();
            // Run startup actions (not needed when registering Elsa with a Host).
            var startupRunner = services.GetRequiredService<IStartupRunner>();
            await startupRunner.StartupAsync();

            // Get a workflow runner.
            var workflowRunner = services.GetRequiredService<IBuildsAndStartsWorkflow>();

            // Run the workflow.
            var runWorkflowResult = await workflowRunner.BuildAndStartWorkflowAsync<HelloWorld>();
            var workflowInstance = runWorkflowResult.WorkflowInstance!;

            // Get a reference to the workflow instance store.
            var store = services.GetRequiredService<IWorkflowInstanceStore>();

            // Count the number of workflow instances of HelloWorld.
            var count = await store.CountAsync(new WorkflowDefinitionIdSpecification(nameof(HelloWorld)));

            Console.WriteLine(count);
            
            var loadedWorkflowInstance = await store.FindByIdAsync(workflowInstance.Id);
            Console.WriteLine(loadedWorkflowInstance);
        }
    }
}