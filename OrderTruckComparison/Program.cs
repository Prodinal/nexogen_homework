using OrderTruckComparison.Algorithms;
using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;
using OrderTruckComparison.Logic;
using System.Diagnostics;

var sw = Stopwatch.StartNew();

const string CONSOLE_OUTPUT_ARGUMENT = "console";

if(args.Length < 2)
{
    Console.WriteLine("Missing arguments inputFile location or output file location");
    Environment.Exit(1);
}

var outputFilePath = args[1];
if (string.IsNullOrEmpty(outputFilePath) || (outputFilePath != CONSOLE_OUTPUT_ARGUMENT && outputFilePath.IndexOfAny(Path.GetInvalidPathChars()) != -1))
{
    Console.WriteLine("Invalid output location argument");
    Environment.Exit(1);
}

var inputFilePath = args[0];
if (!File.Exists(inputFilePath))
{
    Console.WriteLine("Input file path does not exist");
    Environment.Exit(1);
}
var lines = File.ReadLines(inputFilePath);

IInputParser inputParser = new InputParser();
if(!inputParser.ReadInput(lines, out var trucks, out var jobs))
{
    Console.WriteLine("Failed to parse input file");
    Environment.Exit(1);
}

var truckJobPairer = new TruckJobPairer(new MaxBipartiteMatcher());
List<KeyValuePair<Truck, Job>> result;
try
{
    result = truckJobPairer.CalculateBestPairing(trucks, jobs);
}
catch(Exception ex)
{
    Console.WriteLine("Failed to find best matching: " + ex.Message);
    Environment.Exit(1);
    return; //only return here to avoid "result" not initialised warnings
}

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