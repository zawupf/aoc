using System;
using System.Linq;
using Xunit;

namespace Aoc._2019._11.Tests
{
    public class Run_Should
    {
        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("2276", run.Job1());
            Assert.Equal(
                "\n  ##  ###  #    ###    ## ####  ##  #  #   " +
                "\n #  # #  # #    #  #    #    # #  # #  #   " +
                "\n #    ###  #    #  #    #   #  #    #  #   " +
                "\n #    #  # #    ###     #  #   #    #  #   " +
                "\n #  # #  # #    #    #  # #    #  # #  #   " +
                "\n  ##  ###  #### #     ##  ####  ##   ##    ",
                run.Job2()
            );
        }
    }
}
