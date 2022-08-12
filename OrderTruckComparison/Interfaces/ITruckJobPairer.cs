using OrderTruckComparison.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTruckComparison.Interfaces
{
    public interface ITruckJobPairer
    {
        List<KeyValuePair<Truck, Job>> CalculateBestPairing(IReadOnlyCollection<Truck> trucks, IReadOnlyCollection<Job> jobs);
    }
}
