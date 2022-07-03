using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Xunit;
using YellowCanary.Application.Services.DataSources;

namespace YellowCanary.Application.Test.Services.DataSources;

public class ExcelReaderServiceTest
{
    private IExcelReaderService _sut;

    public ExcelReaderServiceTest()
    {
        var fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        _sut = fixture.Create<ExcelReaderService>();
    }

    [Fact]
    public void Should_read_excel_file()
    {
        // Arrange
        const string path = "./TestFiles/Sample Super Data.xlsx";

        // Action
        var (payCodes, payslips, disbursements) = _sut.ReadFile(path);

        // Assert
        payCodes.Should().NotBeEmpty();
        payslips.Should().NotBeEmpty();
        disbursements.Should().NotBeEmpty();
    }
}