namespace OrderTruckComparison.Interfaces
{
    public interface IMaxBipartiteMatcher
    {
        int[] MaxBPM(bool[,] adjacencyMatrix);
    }
}