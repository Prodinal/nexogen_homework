using OrderTruckComparison.Entities;

namespace OrderTruckComparison.Interfaces
{
    internal interface IInputParser
    {
        bool ReadInput(IEnumerable<string> lines, out List<Truck> trucks, out List<Job> jobs);
    }
}
