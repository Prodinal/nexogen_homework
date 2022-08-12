using OrderTruckComparison.Entities;

namespace OrderTruckComparison.Interfaces
{
    public interface IOutputWriter
    {
        void WriteOutput(IEnumerable<KeyValuePair<Truck, Job>> matches);
    }
}
