using OrderTruckComparison.Entities;

namespace OrderTruckComparison.Interfaces
{
    public interface ITruckJobPairer
    {
        List<KeyValuePair<Truck, Job>> CalculateBestPairing(IReadOnlyCollection<Truck> trucks, IReadOnlyCollection<Job> jobs);
    }
}
