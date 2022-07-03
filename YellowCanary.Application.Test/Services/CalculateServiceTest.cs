using AutoFixture;
using AutoFixture.AutoMoq;
using Xunit;
using YellowCanary.Application.Services;
using YellowCanary.Application.Services.DataSources;

namespace YellowCanary.Application.Test.Services;

public class CalculateServiceTest
{
    private ICalculateService _sut;

    public CalculateServiceTest()
    {
        var fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        fixture.Register<IExcelReaderService>(() => fixture.Create<ExcelReaderService>());

        _sut = fixture.Create<CalculateService>();
    }

    [Fact]
    public void Should_calculate()
    {
        // Arrange
        const string path = "./TestFiles/Sample Super Data.xlsx";

        // Action
        _sut.Calculate(path);
    }
}