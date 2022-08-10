using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MaximumFlow;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTruckComparison
{
    public static class VertexExtensions
    {
        public static string GetVertexId(this Truck truck) => "truck-" + truck.Id;
        public static int GetTruckId(this string vertexId) => int.Parse(vertexId.Replace("truck-", ""));
        public static string GetVertexId(this Job job) => "job-" + job.Id;
        public static int GetJobId(this string vertexId) => int.Parse(vertexId.Replace("job-", ""));
    }

    public class TruckJobPairer : ITruckJobPairer
    {
        const string SourceVertexId = "source";
        const string SinkVertexId = "sink";

        public Dictionary<Truck, Job> CalculateBestPairing(List<Truck> trucks, List<Job> jobs)
        {
            //EdmondsKarpTest(trucks, jobs);
            //MaximumPairing(trucks, jobs);
            MaximumBPM(trucks, jobs);

            return new Dictionary<Truck, Job>();
        }

        private Dictionary<Truck, Job> MaximumBPM(List<Truck> trucks, List<Job> jobs)
        {
            var gfg = new GFG();
            var matches = gfg.CalculateMaximumPairing(trucks, jobs);
            Console.WriteLine("IsValidSolution: " + IsValidMatching(matches.ToList()));
            foreach(var match in matches.OrderBy(kvp => kvp.Key.Id))
            {
                Console.WriteLine($"Truck-{match.Key.Id} => Job{match.Value.Id}");
            }

            return new Dictionary<Truck, Job>();
        }


        private Dictionary<Truck, Job> MaximumPairing(List<Truck> trucks, List<Job> jobs)
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();
            var truckVertices = trucks.Select(t => t.GetVertexId()).ToList();
            var jobVertices = jobs.Select(j => j.GetVertexId()).ToList();

            graph.AddVertexRange(truckVertices);
            graph.AddVertexRange(jobVertices);

            foreach(var truck in trucks)
            {
                foreach(var job in jobs)
                {
                    if (truck.CompatibleJobTypes.Contains(job.Type))
                    {
                        graph.AddEdge(new Edge<string>(truck.GetVertexId(), job.GetVertexId()));
                    }
                }
            }

            VertexFactory<string> vertexFactory = () => Guid.NewGuid().ToString();
            EdgeFactory<string, Edge<string>> edgeFactory = (source, target) => new Edge<string>(source, target);

            var maxMatch = new MaximumBipartiteMatchingAlgorithm<string, Edge<string>>(
                graph,
                truckVertices,
                jobVertices,
                vertexFactory,
                edgeFactory
            );
            maxMatch.Compute();

            var result = maxMatch.MatchedEdges
                .Select(e => new KeyValuePair<Truck, Job>(
                    trucks.Single(t => t.Id == e.Source.GetTruckId()),
                    jobs.Single(j => j.Id == e.Target.GetJobId())))
                .ToList();
            var isValid = IsValidMatching(result);

            //var filteredTrucks = new HashSet<string>();
            //var filteredJobs = new HashSet<string>();
            //var filteredEdges = new List<Edge<string>>();
            //foreach (var edge in result)
            //{
            //    if(!filteredTrucks.Contains(edge.Source) && !filteredJobs.Contains(edge.Target))
            //    {
            //        filteredTrucks.Add(edge.Source);
            //        filteredJobs.Add(edge.Target);
            //        filteredEdges.Add(edge);
            //    }
            //}
            //var isValidFiltered = IsValidMatching(filteredEdges.ToArray());

            //Console.WriteLine("Result matching size: " + result.Length);
            //Console.WriteLine("Result isValid: " + isValid);
            //Console.WriteLine("Filtered result size: " + filteredEdges.Count);
            //Console.WriteLine("Filtered result isValid: " + isValidFiltered);
            //Console.WriteLine("All inputTrucks unique: " + (trucks.Select(t => t.Id).Distinct().Count() == trucks.Count));
            //Console.WriteLine("All inputJobs unique: " + (jobs.Select(j => j.Id).Distinct().Count() == jobs.Count));

            return new Dictionary<Truck, Job>();
        }

        private bool IsValidMatching(List<KeyValuePair<Truck, Job>> pairs)
        {
            var uniqueTrucks = pairs.Select(e => e.Key).Distinct().Count() == pairs.Count;
            var uniqueJobs = pairs.Select(e => e.Value).Distinct().Count() == pairs.Count;

            var areMatchesCompatible = pairs.All(pair => pair.Key.CompatibleWith(pair.Value));

            return uniqueTrucks && uniqueJobs && areMatchesCompatible;
        }

        private void EdmondsKarpTest(List<Truck> trucks, List<Job> jobs)
        {
            var graph = CreateGraph(trucks, jobs);
            Func<Edge<string>, double> capacityFunction = edge => 1;   //Each edge has capacity 1

            EdgeFactory<string, Edge<string>> edgeFactory = (source, target) => new Edge<string>(source, target);

            var reversedEdgeAugmentorAlgorithm = new ReversedEdgeAugmentorAlgorithm<string, Edge<string>>(graph, edgeFactory);
            reversedEdgeAugmentorAlgorithm.AddReversedEdges();

            var flow = new EdmondsKarpMaximumFlowAlgorithm<string, Edge<string>>(
                graph,
                capacityFunction,
                edgeFactory,
                reversedEdgeAugmentorAlgorithm);
            flow.Compute(SourceVertexId, SinkVertexId);

            var flowPredecessors = flow.Predecessors.TryGetValue;

            reversedEdgeAugmentorAlgorithm.RemoveReversedEdges();

            var sourcePred = flowPredecessors(SourceVertexId, out var sourceP);
            var sinkPred = flowPredecessors(SinkVertexId, out var sinkP);
            var truckNodePredecessors = new List<(string, bool)>();
            var jobNodePredecessors = new List<(string, bool)>();
            foreach (var truck in trucks)
            {
                var isSuccess = flowPredecessors(truck.GetVertexId(), out var pred);
                truckNodePredecessors.Add((pred.Source + " -> " + pred.Target, isSuccess));
            }
            foreach (var job in jobs)
            {
                var isSuccess = flowPredecessors(job.GetVertexId(), out var pred);
                jobNodePredecessors.Add((pred.Source + " -> " + pred.Target, isSuccess));
            }

            var backtracking = new List<string>();
            var counter = 0;
            foreach (var job in jobs)
            {
                counter++;
                var currentVertex = job.GetVertexId();
                while (flowPredecessors(currentVertex, out var edge))
                {
                    currentVertex = edge.Source;
                    backtracking.Add(counter + ": " + edge.Source + " -> " + edge.Target);
                }
                currentVertex = SinkVertexId;
            }
            backtracking.Reverse();

            var winningEdges = flow.ResidualCapacities
                .Where(edgeCapacityPair => edgeCapacityPair.Value < 0.001f)
                .Where(edge => !edge.Key.Source.Contains(SourceVertexId)
                            && !edge.Key.Target.Contains(SinkVertexId))
                .Select(pair => pair.Key)
                .Select(edge => { Console.WriteLine(edge.Source + " -> " + edge.Target); return edge; })
                .ToList();
        }


        private AdjacencyGraph<string, Edge<string>> CreateGraph(List<Truck> trucks, List<Job> jobs)
        {
            var edges = new List<Edge<string>>();

            foreach (var truck in trucks)
            {
                edges.Add(new Edge<string>(SourceVertexId, truck.GetVertexId()));   //Connect source to each truck vertex
                foreach (var job in jobs.Where(j => truck.CompatibleJobTypes.Contains(j.Type)))  //Connect each trux vertex to all job vertices it is compatible with
                {
                    edges.Add(new Edge<string>(truck.GetVertexId(), job.GetVertexId()));
                }
            }

            foreach (var job in jobs)
            {
                edges.Add(new Edge<string>(job.GetVertexId(), SinkVertexId));   //Connect all job vertices to the sink
            }

            return edges.ToAdjacencyGraph<string, Edge<string>>();
        }


    }
}
