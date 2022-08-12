using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTruckComparison
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteOutput(IEnumerable<KeyValuePair<Truck, Job>> matches)
        {
            foreach(var match in matches)
            {
                Console.WriteLine($"{match.Key.Id} {match.Value.Id}");
            }
        }
    }
}
