using OrderTruckComparison.Entities;

class GFG
{
    // M is number of applicants
    // and N is number of jobs
    private int truckCount;
    private int jobCount;

    // A DFS based recursive function
    // that returns true if a matching
    // for vertex u is possible
    private bool bpm(bool[,] bpGraph, int u, bool[] seen, int[] matchR)
    {
        // Try every job one by one
        for (int v = 0; v < jobCount; v++)
        {
            // If applicant u is interested
            // in job v and v is not visited
            if (bpGraph[u, v] && !seen[v])
            {
                // Mark v as visited
                seen[v] = true;

                // If job 'v' is not assigned to
                // an applicant OR previously assigned
                // applicant for job v (which is matchR[v])
                // has an alternate job available.
                // Since v is marked as visited in the above
                // line, matchR[v] in the following recursive
                // call will not get job 'v' again
                if (matchR[v] < 0 || bpm(bpGraph, matchR[v],
                                         seen, matchR))
                {
                    matchR[v] = u;
                    return true;
                }
            }
        }
        return false;
    }

    // Returns maximum number of
    // matching from M to N
    private int[] maxBPM(bool[,] bpGraph)
    {
        truckCount = bpGraph.GetLength(0);
        jobCount = bpGraph.GetLength(1);

        // An array to keep track of the
        // applicants assigned to jobs.
        // The value of matchR[i] is the
        // applicant number assigned to job i,
        // the value -1 indicates nobody is assigned.
        int[] matchR = new int[jobCount];

        // Initially all jobs are available
        for (int i = 0; i < jobCount; ++i)
            matchR[i] = -1;

        // Count of jobs assigned to applicants
        int result = 0;
        for (int u = 0; u < truckCount; u++)
        {
            // Mark all jobs as not
            // seen for next applicant.
            bool[] seen = new bool[jobCount];
            for (int i = 0; i < jobCount; ++i)
                seen[i] = false;

            // Find if the applicant
            // 'u' can get a job
            if (bpm(bpGraph, u, seen, matchR))
                result++;
        }
        return matchR;
    }

    public Dictionary<Truck, Job> CalculateMaximumPairing(List<Truck> trucks, List<Job> jobs)
    {
        var matrix = new bool[trucks.Count,jobs.Count];
        for(var i = 0; i < trucks.Count; i++)
        {
            for (var j = 0; j < jobs.Count; j++)
            {
                matrix[i, j] = trucks[i].CompatibleWith(jobs[j]);
            }
        }

        var matches = maxBPM(matrix);
        
        var result = new Dictionary<Truck, Job>();
        for (int i = 0; i < matches.Length; i++)
        {
            if(matches[i] != -1)
            {
                var matchedTruck = trucks[matches[i]];
                var matchedJob = jobs[i];
                result.Add(matchedTruck, matchedJob);
            }
        }
        return result;
    }
}