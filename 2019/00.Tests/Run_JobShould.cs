using Xunit;

namespace Aoc._2019._00.Test
{
    public class Run_JobShould
    {
        private readonly Run _run = new Run();

        [Fact]
        public void Job1_ReturnString()
        {
            var result = _run.Job1();
            Assert.IsType<string>(result);
            Assert.Contains("job1", result);
        }

        [Fact]
        public void Job2_ReturnString()
        {
            var result = _run.Job2();
            Assert.IsType<string>(result);
            Assert.Contains("job2", result);
        }
    }
}
