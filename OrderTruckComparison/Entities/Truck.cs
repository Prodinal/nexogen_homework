using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTruckComparison.Entities
{
    public record Truck
    {
        public int Id { get; init; }
        public List<string> CompatibleJobTypes { get; init; } = new List<string>();
    }
}
