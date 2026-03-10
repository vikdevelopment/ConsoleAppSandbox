using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        Console.WriteLine($"Environment: {context.HostingEnvironment.EnvironmentName}");
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        config
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: false)
            .AddEnvironmentVariables();

        var builtConfig = config.Build();
    })
    .Build();

await RunAsync(host.Services);

static async Task RunAsync(IServiceProvider services)
{
    //load variables:
    var configuration = services.GetRequiredService<IConfiguration>();
    var systemEnvironment = configuration["SYSTEM_ENVIRONMENT"];

    var testValue = configuration["Test"];

    Console.Write("Console application started successfully.");
    Console.Write($"Variable test: {testValue}");

    Console.Write("Console application finished successfully.");
    await Task.CompletedTask;
}