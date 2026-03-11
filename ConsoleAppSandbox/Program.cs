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
    Console.WriteLine("Console application started successfully. Version 1");

    //load variables:
    var configuration = services.GetRequiredService<IConfiguration>();

    var systemEnvironment = configuration["SYSTEM_ENVIRONMENT"];

    //TEST: load variable from appsettings.{Environment}.json
    var appSetValue = configuration["AppSet"];
    Console.WriteLine($"Variable AppSet: {appSetValue}");

    //Test: load from envirnomantal variable from dockerFile:
    var runEveryValue = configuration["RUN_EVERY_MINUTES"];
    Console.WriteLine($"Variable runEveryValue: {runEveryValue}");

    //Test: added special variable in azure portal:
    var onlyInAzure = configuration["ONLY_AZURE"];
    Console.WriteLine($"Variable onlyInAzure: {onlyInAzure}");

    //Test: obly in appSettings:
    var onlyInAppset = configuration["OnlyAppSet"];
    Console.WriteLine($"Variable onlyInAppSettings: {onlyInAppset}");
    

    Console.WriteLine("Console application finished successfully.");
    
    await Task.CompletedTask;
}