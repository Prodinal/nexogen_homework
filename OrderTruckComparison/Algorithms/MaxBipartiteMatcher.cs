using OrderTruckComparison.Interfaces;

namespace OrderTruckComparison.Algorithms
{
    //Code originally based on this https://www.geeksforgeeks.org/maximum-bipartite-matching/
    public class MaxBipartiteMatcher : IMaxBipartiteMatcher
    {
        private int truckCount;
        private int jobCount;

        // A DFS based recursive function that returns true if a matching for vertex u is possible, and updates matchR accordingly 
        private bool CanTruckGetJob(bool[,] adjacencyMatrix, int truckIndex, bool[] seen, int[] jobMatches)
        {
            // Try every job one by one
            for (int jobIndex = 0; jobIndex < jobCount; jobIndex++)
            {
                // If applicant u is interested
                // in job v and v is not visited
                if (adjacencyMatrix[truckIndex, jobIndex] && !seen[jobIndex])
                {
                    // Mark v as visited
                    seen[jobIndex] = true;

                    // If job 'jobIndex' is not assigned to
                    // a truck OR previously assigned
                    // truck for job jobIndex (which is jobMatches[jobIndex])
                    // has an alternate job available.
                    // Since jobIndex is marked as visited in the above
                    // line, jobMatches[jobIndex] in the following recursive
                    // call will not get job 'jobIndex' again
                    if (jobMatches[jobIndex] < 0 || CanTruckGetJob(adjacencyMatrix, jobMatches[jobIndex], seen, jobMatches))
                    {
                        jobMatches[jobIndex] = truckIndex;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns an array of integers, where [i] means the index of the truck that is assigned to the job with index i
        /// E.g. [-1, 2, 1] means job0 has no truck assigned to it, job1 has truck under index2 assigned to it, and job2 has truck under index1 assigned to it
        /// </summary>
        /// <param name="adjacencyMatrix">Adjacency matrix of graph, [i,j] true means there is an edge between vertex vertex i and vertex j</param>
        /// <returns></returns>
        public int[] MaxBPM(bool[,] adjacencyMatrix)
        {
            truckCount = adjacencyMatrix.GetLength(0);
            jobCount = adjacencyMatrix.GetLength(1);

            // An array to keep track of the
            // trucks assigned to jobs.
            // The value of jobMatches[i] is the
            // truck index assigned to job i,
            // the value -1 indicates nobody is assigned.
            int[] jobMatches = new int[jobCount];

            // Initially all jobs are available
            for (int i = 0; i < jobCount; ++i)
            {
                jobMatches[i] = -1;
            }

            for (int truckIndex = 0; truckIndex < truckCount; truckIndex++)
            {
                // Mark all jobs as not
                // seen for next applicant.
                bool[] seen = new bool[jobCount];
                for (int i = 0; i < jobCount; ++i)
                    seen[i] = false;

                // Find if the applicant
                // 'u' can get a job
                _ = CanTruckGetJob(adjacencyMatrix, truckIndex, seen, jobMatches);
            }
            return jobMatches;
        }
    }
}