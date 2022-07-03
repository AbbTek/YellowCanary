using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using YellowCanary.Application.Services;
using YellowCanary.Application.Services.DataSources;
using YellowCanary.Console.Configuration;

namespace YellowCanary.Console
{
    internal static class Program
    {
        private static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            ProgramOptions options = null;
            Parser.Default.ParseArguments<ProgramOptions>(args).WithParsed(o => options = o);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .AddSingleton(options)
                        .AddScoped<IExcelReaderService, ExcelReaderService>()
                        .AddScoped<ICalculateService, CalculateService>()
                        .AddHostedService<MainService>())
                .UseSerilog();
        }
    }
}