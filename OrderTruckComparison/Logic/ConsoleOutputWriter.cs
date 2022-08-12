using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;

namespace OrderTruckComparison.Logic
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteOutput(IEnumerable<KeyValuePair<Truck, Job>> matches)
        {
            foreach (var match in matches)
            {
                Console.WriteLine($"{match.Key.Id} {match.Value.Id}");
            }
        }
    }
}
