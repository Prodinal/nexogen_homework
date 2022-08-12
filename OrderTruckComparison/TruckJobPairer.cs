using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;
using System.Diagnostics;

namespace OrderTruckComparison
{
    public class TruckJobPairer : ITruckJobPairer
    {
        public List<KeyValuePair<Truck, Job>> CalculateBestPairing(IReadOnlyCollection<Truck> trucks, IReadOnlyCollection<Job> jobs)
        {
            var matcher = new MaxBipartiteMatcher();

            var adjacencyMatrix = CreateMatrix(trucks, jobs);
            var matches = matcher.MaxBPM(adjacencyMatrix);
            var truckJobPairs = ConvertMatchesToTruckJobPairs(matches, trucks, jobs);

            var isValid = IsValidMatching(truckJobPairs);

            Debug.WriteLine($"Found {truckJobPairs.Count} matches");
            Debug.WriteLine("Is solution valid: " + isValid);

            //Sanity check
            if (!isValid)
            {
                Console.WriteLine("WARNING - Something went wrong, the solution found is not valid :c");
            }

            return truckJobPairs;
        }

        private static List<KeyValuePair<Truck, Job>> ConvertMatchesToTruckJobPairs(int[] matches, IReadOnlyCollection<Truck> trucks, IReadOnlyCollection<Job> jobs)
        {
            var result = new List<KeyValuePair<Truck, Job>>();
            for (int i = 0; i < matches.Length; i++)
            {
                if (matches[i] != -1)
                {
                    var matchedTruck = trucks.ElementAt(matches[i]);
                    var matchedJob = jobs.ElementAt(i);
                    result.Add(new KeyValuePair<Truck, Job>(matchedTruck, matchedJob));
                }
            }
            return result;
        }

        private static bool[,] CreateMatrix(IReadOnlyCollection<Truck> trucks, IReadOnlyCollection<Job> jobs)
        {
            var matrix = new bool[trucks.Count, jobs.Count];
            for (var i = 0; i < trucks.Count; i++)
            {
                for (var j = 0; j < jobs.Count; j++)
                {
                    matrix[i, j] = trucks.ElementAt(i).CompatibleWith(jobs.ElementAt(j));
                }
            }

            return matrix;
        }

        private static bool IsValidMatching(List<KeyValuePair<Truck, Job>> pairs)
        {
            var uniqueTrucks = pairs.Select(e => e.Key).Distinct().Count() == pairs.Count;
            var uniqueJobs = pairs.Select(e => e.Value).Distinct().Count() == pairs.Count;

            var areMatchesCompatible = pairs.All(pair => pair.Key.CompatibleWith(pair.Value));

            return uniqueTrucks && uniqueJobs && areMatchesCompatible;
        }
    }
}
