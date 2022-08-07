namespace OrderTruckComparison.Test
{
    public class Tests
    {
        private InputParser inputParser;

        [SetUp]
        public void Setup()
        {
            inputParser = new InputParser();
        }

        [Test]
        public void Test1()
        {
            var lines = new[] 
            {
                "3",
                "1 A B",
                "2 B",
                "3 B",
                "2",
                "1 B",
                "2 A",
            };

            inputParser.ReadInput(lines, out var trucks, out var jobs);
            Assert.Multiple(() =>
            {
                Assert.That(trucks, Has.Count.EqualTo(3));
                Assert.That(trucks.Single(t => t.Id == 1).CompatibleJobTypes, Does.Contain("A"));
                Assert.That(trucks.Single(t => t.Id == 1).CompatibleJobTypes, Does.Contain("B"));
                Assert.That(trucks.Single(t => t.Id == 2).CompatibleJobTypes, Does.Contain("B"));
                Assert.That(trucks.Single(t => t.Id == 3).CompatibleJobTypes, Does.Contain("B"));

                Assert.That(jobs, Has.Count.EqualTo(2));
                Assert.That(jobs.Single(j => j.Id == 1).Type, Is.EqualTo("B"));
                Assert.That(jobs.Single(j => j.Id == 2).Type, Is.EqualTo("A"));
            });
        }
    }
}