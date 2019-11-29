using System;
using Xunit;

namespace Aoc._2019._00.Test
{
    public class Run_JobShould
    {
        private readonly Run _run = new Run();

        [Fact]
        public void Job_ReturnString()
        {
            Assert.IsType<string>(_run.Job1());
        }
    }
}
