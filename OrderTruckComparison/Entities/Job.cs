using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTruckComparison.Entities
{
    public record Job
    {
        public int Id { get; init; }
        public string Type { get; init; } = string.Empty;
    }
}
