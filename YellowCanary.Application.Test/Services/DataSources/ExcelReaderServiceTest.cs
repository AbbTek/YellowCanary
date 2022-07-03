using AutoFixture;
using AutoFixture.AutoMoq;
using Xunit;
using YellowCanary.Application.Services.DataSources;

namespace YellowCanary.Application.Test.Services.DataSources;

public class ExcelReaderServiceTest
{
    private IFixture _fixture;

    private IExcelReaderService _sut;

    public ExcelReaderServiceTest()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        _sut = _fixture.Create<ExcelReaderService>();
    }

    [Fact]
    public void Should_read_excel_file()
    {
        _sut.ReadFile("./TestFiles/Sample Super Data.xlsx");
    }
}