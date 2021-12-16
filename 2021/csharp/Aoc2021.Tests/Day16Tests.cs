namespace Aoc2021.Tests;

public class Day16Tests
{
    [Fact]
    public void VersionSumWorks()
    {
        Assert.Equal(16UL, Day16.VersionSum("8A004A801A8002F478"));
        Assert.Equal(12UL, Day16.VersionSum("620080001611562C8802118E34"));
        Assert.Equal(23UL, Day16.VersionSum("C0015000016115A2E0802F182340"));
        Assert.Equal(31UL, Day16.VersionSum("A0016C880162017C3686B18A3D4780"));
    }

    [Fact]
    public void ValueWorks()
    {
        Assert.Equal(3UL, Day16.Value("C200B40A82"));
        Assert.Equal(54UL, Day16.Value("04005AC33890"));
        Assert.Equal(7UL, Day16.Value("880086C3E88112"));
        Assert.Equal(9UL, Day16.Value("CE00C43D881120"));
        Assert.Equal(1UL, Day16.Value("D8005AC2A8F0"));
        Assert.Equal(0UL, Day16.Value("F600BC2D8F"));
        Assert.Equal(0UL, Day16.Value("9C005AC2F8F0"));
        Assert.Equal(1UL, Day16.Value("9C0141080250320F1802104A08"));
    }

    [Fact]
    public void Stars()
    {
        Day16 run = new();
        Assert.Equal("949", run.Result1());
        Assert.Equal("1114600142730", run.Result2()); // WRONG: 2203613086
    }
}
