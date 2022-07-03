using CommandLine;

namespace YellowCanary.Console.Configuration;

public class ProgramOptions
{
    [Option('p', "path", Required = true, HelpText = "Path to the excel file")]
    public string Path { get; set; }
}