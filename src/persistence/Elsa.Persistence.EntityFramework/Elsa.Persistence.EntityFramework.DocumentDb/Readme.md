### Usage

```
public void ConfigureServices(IServiceCollection services)
{

    services.AddElsa(elsa =>
        elsa
            .AddWorkflow<MyWorkflow>()
            .UseEntityFrameworkPersistence(
                contextOptions =>
                {
                    contextOptions.UseDocumentDb(
                        "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        "Elsa");
                },
                autoRunMigrations: false)
    );
}
```