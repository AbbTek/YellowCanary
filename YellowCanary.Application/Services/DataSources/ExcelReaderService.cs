using System.Data;
using System.Text;
using ExcelDataReader;
using Microsoft.Extensions.Logging;

namespace YellowCanary.Application.Services.DataSources;

public interface IExcelReaderService
{
    void ReadFile(string path);
}

public class ExcelReaderService : IExcelReaderService
{
    private readonly ILogger<ExcelReaderService> _logger;
    

    public ExcelReaderService(ILogger<ExcelReaderService> logger)
    {
        _logger = logger;
    }

    public void ReadFile(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        
        var result = reader.AsDataSet();

        foreach (DataTable table in result.Tables)
        {
            _logger.LogInformation("Table {@TableName}", table.Namespace);
        }

        _logger.LogInformation("First column {@Result}", result);
    }

    private void ProcessTable()
    {
        
    }
}