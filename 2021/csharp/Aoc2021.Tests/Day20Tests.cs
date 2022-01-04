namespace Aoc2021.Tests;

public class Day20Tests
{
    private static readonly List<string> input = @"
        ..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

        #..#.
        #....
        ##..#
        ..#..
        ..###
    "
    .Lines();

    [Fact]
    public void PixelCountAfter2EnhancementsWorks()
    {
        Assert.Equal(35, Day20.ImageProcessor.Parse(input).Enhancement(2).Pixels.Count);
    }

    [Fact]
    public void PixelCountAfter50EnhancementsWorks()
    {
        Assert.Equal(3351, Day20.ImageProcessor.Parse(input).Enhancement(50).Pixels.Count);
    }

    [Fact]
    public void Stars()
    {
        Day20 run = new();
        Assert.Equal("5573", run.Result1());
        Assert.Equal("20097", run.Result2());
    }
}
