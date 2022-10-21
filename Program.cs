using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoggersComparison
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .WriteTo.Async(a => a.File("logs/seriLogger.log"), bufferSize: 128000)
                .CreateLogger();          
            
            var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddSingleton(Log.Logger);

                services.AddTransient<ILoggerComparison, LoggerComarison>();
                services.AddSingleton<IFileLogger, FileLogger>();

            })
            .UseSerilog()
            .Build();

            var svc = ActivatorUtilities.CreateInstance<LoggerComarison>(host.Services);
            await svc.Compare();
            Log.CloseAndFlush();
        }



        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
