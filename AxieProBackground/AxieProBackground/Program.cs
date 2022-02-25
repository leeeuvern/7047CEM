using AxiePro.Services;
using AxieProBackground;
using AxieProBackround.Services;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        //     services.AddHostedService<Worker>();
        services.AddHttpClient();
        services.AddSingleton<IAxieDataFactory, AxieDataFactory>();
        services.AddSingleton<IAxieApiService, AxieApiService>();
        var connectionString = "server=localhost;user=root;password=root;database=axiepro";

        // Replace with your server version and type.
        // Use 'MariaDbServerVersion' for MariaDB.
        // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
        // For common usages, see pull request #1233.
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

        // Replace 'YourDbContext' with the name of your own DbContext derived class.
        services.AddDbContext<DataContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
    })
    .Build();

await host.RunAsync();
