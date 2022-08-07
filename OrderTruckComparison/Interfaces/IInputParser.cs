using OrderTruckComparison.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTruckComparison.Interfaces
{
    internal interface IInputParser
    {
        bool ReadInput(IEnumerable<string> lines, out List<Truck> trucks, out List<Job> jobs);
    }
}
