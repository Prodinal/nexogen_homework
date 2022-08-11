using System.Text;


const string FileName = "generated_sample.txt";
const string JobTypes = "ABCDEFGHIJKLMNOPQRTSUVWXYZ";

int howManyTrucks;
int howManyJobs;
int howManyJobTypes;

try
{
    howManyTrucks = int.Parse(args[0]);
    howManyJobs = int.Parse(args[1]);
    howManyJobTypes = int.Parse(args[2]);
}
catch
{
    Console.WriteLine("Pass 3 integer arguments: number of trucks, number of jobs, number of jobTypes");
    return;
}

if(howManyJobTypes >= JobTypes.Length)
{
    Console.WriteLine($"Job types cannot be more than {JobTypes.Length}");
    return;
}

var possibleJobTypes = JobTypes[..howManyJobTypes];
var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
Console.WriteLine("Writing output file to " + filePath);
using (var sw = new StreamWriter(filePath, false))
{
    Random rudi = new();

    sw.WriteLine(howManyTrucks);
    for (int i = 0; i < howManyTrucks; i++)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"{i+1}");

        var howManyCompatibleJobs = rudi.Next(1, 4);    //Allow at least 1, max 3 compatible types for a truck
        for (int j = 0; j < howManyCompatibleJobs; j++)
        {
            var randomJobType = rudi.Next(0, possibleJobTypes.Length);
            stringBuilder.Append($" {possibleJobTypes[randomJobType]}");
        }

        sw.WriteLine(stringBuilder.ToString());
    }
    sw.WriteLine(howManyJobs);
    for (int i = 0; i < howManyJobs; i++)
    {
        var randomJobType = rudi.Next(0, possibleJobTypes.Length);
        var jobLine = $"{i + 1} {possibleJobTypes[randomJobType]}";

        sw.WriteLine(jobLine);
    }
}
