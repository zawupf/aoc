using System;
using System.Linq;
using Xunit;

namespace Aoc._2019._16.Tests
{
    public class Run_Should
    {
        [Fact]
        public void FFT_simple_example_works()
        {
            var fft = new FFT();
            fft.Phases("12345678").ElementAt(0);
            Assert.Equal("48226158", fft.Current(0, 8));
            fft.Phases("12345678").ElementAt(1);
            Assert.Equal("34040438", fft.Current(0, 8));
            fft.Phases("12345678").ElementAt(2);
            Assert.Equal("03415518", fft.Current(0, 8));
            fft.Phases("12345678").ElementAt(3);
            Assert.Equal("01029498", fft.Current(0, 8));
        }

        [Theory]
        [InlineData("24176176", "80871224585914546619083218645595")]
        [InlineData("73745418", "19617804207202209144916044189917")]
        [InlineData("52432133", "69317163492948606335995924319873")]
        public void FFT_Phases_works(string expected, string input)
        {
            var fft = new FFT();
            var result = fft.Phases(input).ElementAt(99);
            Assert.Equal(expected, fft.Current(0, 8));
        }

        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("96136976", run.Job1());
            // Assert.Equal("", run.Job2());
        }
    }
}
