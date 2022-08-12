using OrderTruckComparison;
using OrderTruckComparison.Interfaces;
using System.Diagnostics;

var sw = Stopwatch.StartNew();

const string CONSOLE_OUTPUT_ARGUMENT = "console";

if(args.Length < 2)
{
    Console.WriteLine("Missing arguments inputFile location or output file location");
    return;
}

var outputFilePath = args[1];
if (string.IsNullOrEmpty(outputFilePath) || (outputFilePath != CONSOLE_OUTPUT_ARGUMENT && outputFilePath.IndexOfAny(Path.GetInvalidPathChars()) != -1))
{
    Console.WriteLine("Invalid output location argument");
    return;
}

var inputFilePath = args[0];
if (!File.Exists(inputFilePath))
{
    Console.WriteLine("Input file path does not exist");
    return;
}
var lines = File.ReadLines(inputFilePath);

IInputParser inputParser = new InputParser();
if(!inputParser.ReadInput(lines, out var trucks, out var jobs))
{
    Console.WriteLine("Failed to parse input file");
    return;
}

var truckJobPairer = new TruckJobPairer();
var result = truckJobPairer.CalculateBestPairing(trucks, jobs);

IOutputWriter outputWriter;
if(outputFilePath != CONSOLE_OUTPUT_ARGUMENT)
{
    outputWriter = new FileOutputWriter(outputFilePath);
} else
{
    outputWriter = new ConsoleOutputWriter();
}
outputWriter.WriteOutput(result.OrderBy(kvp =>kvp.Key.Id));


Debug.WriteLine($"Finished running in {sw.Elapsed.TotalSeconds}seconds");