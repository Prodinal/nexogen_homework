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
        Dictionary<Truck, Job> CalculateBestPairing(List<Truck> trucks, List<Job> jobs);
    }
}
