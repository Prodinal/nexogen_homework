namespace OrderTruckComparison.Entities
{
    public record Truck
    {
        public int Id { get; init; }
        public List<string> CompatibleJobTypes { get; init; } = new List<string>();

        public bool CompatibleWith(Job job)
        {
            return CompatibleJobTypes.Contains(job.Type);
        }
    }
}
