namespace OrderTruckComparison.Entities
{
    public record Job
    {
        public int Id { get; init; }
        public string Type { get; init; } = string.Empty;
    }
}
