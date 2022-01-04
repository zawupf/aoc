namespace Aoc2021.Tests;

public class Day13Tests
{
    private static readonly List<string> input = @"
        6,10
        0,14
        9,10
        0,3
        10,4
        4,11
        6,0
        6,12
        4,1
        0,13
        10,12
        3,4
        3,0
        8,4
        1,10
        2,14
        8,10
        9,0

        fold along y=7
        fold along x=5
    "
    .Lines();

    [Fact]
    public void DotCount1Works()
    {
        Assert.Equal(17, Day13.DotCount(input).First());
        Assert.Equal(16, Day13.DotCount(input).Skip(1).First());
    }

    [Fact]
    public void Stars()
    {
        Day13 run = new();
        Assert.Equal("684", run.Result1());
        Assert.Equal(@"
  ## ###  #### ###  #     ##  #  # #  #
   # #  #    # #  # #    #  # # #  #  #
   # #  #   #  ###  #    #    ##   ####
   # ###   #   #  # #    # ## # #  #  #
#  # # #  #    #  # #    #  # # #  #  #
 ##  #  # #### ###  ####  ### #  # #  #", run.Result2());
    }
}
