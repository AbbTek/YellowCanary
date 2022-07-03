using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using YellowCanary.Application.Services;
using YellowCanary.Console.Configuration;

namespace YellowCanary.Console;

public class MainService : BackgroundService
{
    private readonly ProgramOptions _options;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<MainService> _logger;
    private readonly ICalculateService _calculateService;

    public MainService(
        ProgramOptions options,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<MainService> logger,
        ICalculateService calculateService)
    {
        _options = options;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _calculateService = calculateService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(() =>
        {
            try
            {
                var result = _calculateService.Calculate(_options.Path);

                foreach (var line in result)
                {
                    _logger.LogInformation(
                        "Quarter {@Quarter} EmployeeCode {@EmployeeCode} Total OTE {@Ote:N0} Total Disbursement {@Disbursement:N0}",
                        line.Quarter,
                        line.EmployedId, line.TotalOte, line.TotalDisbursement);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled Exception");
            }
            finally
            {
                Log.CloseAndFlush();
                _hostApplicationLifetime.StopApplication();
            }
        }, stoppingToken);
}