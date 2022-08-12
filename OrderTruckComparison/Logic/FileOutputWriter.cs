using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;

namespace OrderTruckComparison.Logic
{
    public class FileOutputWriter : IOutputWriter
    {
        private readonly string _outputPath;

        public FileOutputWriter(string outputPath)
        {
            _outputPath = outputPath;
        }

        public void WriteOutput(IEnumerable<KeyValuePair<Truck, Job>> matches)
        {
            using var streamWriter = new StreamWriter(_outputPath, false);
            foreach (var match in matches)
            {
                streamWriter.WriteLine($"{match.Key.Id} {match.Value.Id}");
            }
            streamWriter.Close();
        }
    }
}
