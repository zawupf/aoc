module Day20.Tests

open Xunit
open Utils
open Day20

[<Theory>]
[<InlineData(10, 1)>]
[<InlineData(30, 2)>]
[<InlineData(40, 3)>]
[<InlineData(70, 4)>]
[<InlineData(60, 5)>]
[<InlineData(120, 6)>]
[<InlineData(80, 7)>]
[<InlineData(150, 8)>]
[<InlineData(130, 9)>]
let ``Day20 present count works`` expected house =
    Assert.Equal(expected, presentCount1 () |> Seq.item (house - 1))

[<Theory>]
[<InlineData(10, 1)>]
[<InlineData(30, 2)>]
[<InlineData(40, 3)>]
[<InlineData(70, 4)>]
[<InlineData(60, 4)>]
[<InlineData(120, 6)>]
[<InlineData(80, 6)>]
[<InlineData(150, 8)>]
[<InlineData(130, 8)>]
let ``Day20 findFirstHouseWithAtLeast works`` limit house =
    Assert.Equal(house, limit |> findFirstHouseWithAtLeast presentCount1)

[<Fact>]
let ``Day20 Stars`` () =
    try
        Assert.Equal("665280", job1 ())
        Assert.Equal("705600", job2 ())
    with :? System.NotImplementedException ->
        ()
