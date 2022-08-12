using OrderTruckComparison.Entities;
using OrderTruckComparison.Interfaces;

namespace OrderTruckComparison.Logic
{
    public class InputParser : IInputParser
    {

        public bool ReadInput(IEnumerable<string> lines, out List<Truck> trucks, out List<Job> jobs)
        {
            IEnumerator<string>? iterator = null;
            try
            {
                trucks = new List<Truck>();
                jobs = new List<Job>();

                iterator = lines.GetEnumerator();

                if (!iterator.MoveNext()) throw new Exception("Empty input file");  //First line: number of trucks
                if(!int.TryParse(iterator.Current, out var truckCount))
                {
                    throw new Exception($"Failed to parse {iterator.Current} as truck count");
                }
                for (var i = 0; i < truckCount; i++)
                {
                    if (!iterator.MoveNext()) throw new Exception("Fewer truck lines than the specified " + truckCount);    //Read each line of truck
                    trucks.Add(GetTruckFromLine(iterator.Current));
                }

                if (!iterator.MoveNext()) throw new Exception("Job count missing");     //First line after trucks: number of jobs
                if(!int.TryParse(iterator.Current, out var jobCount))
                {
                    throw new Exception($"Failed to parse {iterator.Current} as job count");
                }
                for (var i = 0; i < jobCount; i++)
                {
                    if (!iterator.MoveNext()) throw new Exception("Fewer job lines than the specified " + jobCount);        //Read each line of jobs
                    jobs.Add(GetJobFromLine(iterator.Current));
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                trucks = new List<Truck>();
                jobs = new List<Job>();
                return false;
            }
            finally
            {
                if (iterator != null)
                {
                    iterator.Dispose();
                }
            }
        }

        private Job GetJobFromLine(string line)
        {
            var parts = line.Split(' ');
            if (parts.Length != 2) throw new Exception("Malformed job line: " + line);

            return new Job
            {
                Id = int.Parse(parts[0]),
                Type = parts[1]
            };
        }

        private Truck GetTruckFromLine(string line)
        {
            var parts = line.Split(' ');
            if (parts.Length < 2) throw new Exception("Malformed truck line: " + line);

            return new Truck
            {
                Id = int.Parse(parts.First()),
                CompatibleJobTypes = parts.Skip(1).ToList(),
            };
        }
    }
}
