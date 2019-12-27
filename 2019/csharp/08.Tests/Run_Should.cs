using System;
using Xunit;

namespace Aoc._2019._08.Tests
{
    public class Run_Should
    {
        [Fact]
        public void Image_Checksum_Works()
        {
            Assert.Equal(1, Image.Parse(3, 2, "123456789012").Checksum());
        }

        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("2806", run.Job1());
            Assert.Equal(
                "\n**** ***    **  **  ***  " +
                "\n   * *  *    * *  * *  * " +
                "\n  *  ***     * *  * ***  " +
                "\n *   *  *    * **** *  * " +
                "\n*    *  * *  * *  * *  * " +
                "\n**** ***   **  *  * ***  ",
                run.Job2());
        }
    }
}
