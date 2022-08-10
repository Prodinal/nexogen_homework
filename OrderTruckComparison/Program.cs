using OrderTruckComparison;
using OrderTruckComparison.Interfaces;
using System.Diagnostics;

var sw = Stopwatch.StartNew();
if(args.Length == 0)
{
    Console.WriteLine("Missing argument");
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


Console.WriteLine($"Finished running in {sw.Elapsed.TotalSeconds}seconds");