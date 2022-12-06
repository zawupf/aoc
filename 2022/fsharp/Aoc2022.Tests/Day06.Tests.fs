module Day06.Tests

open Xunit
open Utils
open Day06

[<Theory>]
[<InlineData(7, "mjqjpqmgbljsphdztnvjfqwrcgsmlb")>]
[<InlineData(5, "bvwbjplbgvbhsrlpgdmjqwftvncz")>]
[<InlineData(6, "nppdvjthqldpwncqszvftbrmjlhg")>]
[<InlineData(10, "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg")>]
[<InlineData(11, "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw")>]
let ``Day06 countCharsForStartOfPacketMarker works`` expected signal =
    Assert.Equal(expected, signal |> countCharsForStartOfPacketMarker)

[<Theory>]
[<InlineData(19, "mjqjpqmgbljsphdztnvjfqwrcgsmlb")>]
[<InlineData(23, "bvwbjplbgvbhsrlpgdmjqwftvncz")>]
[<InlineData(23, "nppdvjthqldpwncqszvftbrmjlhg")>]
[<InlineData(29, "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg")>]
[<InlineData(26, "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw")>]
let ``Day06 countCharsForStartOfMessageMarker works`` expected signal =
    Assert.Equal(expected, signal |> countCharsForStartOfMessageMarker)

[<Fact>]
let ``Day06 Stars`` () =
    try
        Assert.Equal("1480", job1 ())
        Assert.Equal("2746", job2 ())
    with :? System.NotImplementedException ->
        ()
