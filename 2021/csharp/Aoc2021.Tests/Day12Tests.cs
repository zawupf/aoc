namespace Aoc2021.Tests;

public class Day12Tests
{
    private static readonly List<string> input1 = @"
        start-A
        start-b
        A-c
        A-b
        b-d
        A-end
        b-end
    "
    .Lines();

    private static readonly List<string> input2 = @"
        dc-end
        HN-start
        start-kj
        dc-start
        dc-HN
        LN-dc
        HN-end
        kj-sa
        kj-HN
        kj-dc
    "
    .Lines();

    private static readonly List<string> input3 = @"
        fs-end
        he-DX
        fs-he
        start-DX
        pj-DX
        end-zg
        zg-sl
        zg-pj
        pj-he
        RW-he
        fs-DX
        pj-RW
        zg-RW
        start-pj
        he-WI
        zg-he
        pj-fs
        start-RW
    "
    .Lines();

    [Fact]
    public void PathCountWorks()
    {
        Assert.Equal(10, Day12.PathCount(input1));
        Assert.Equal(19, Day12.PathCount(input2));
        Assert.Equal(226, Day12.PathCount(input3));
    }

    [Fact]
    public void FirstStepFullFlashWorks()
    {
        Assert.Equal(36, Day12.ExtraPathCount(input1));
        Assert.Equal(103, Day12.ExtraPathCount(input2));
        Assert.Equal(3509, Day12.ExtraPathCount(input3));
    }

    [Fact]
    public void Stars()
    {
        Day12 run = new();
        Assert.Equal("3708", run.Result1());
        Assert.Equal("93858", run.Result2());
    }
}
