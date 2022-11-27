module Day15.Tests

open Xunit
open Utils
open Day15

[<Theory>]
[<InlineData(0, 0, 3, 0)>]
[<InlineData(1, 0, 3, 1)>]
[<InlineData(2, 0, 3, 2)>]
[<InlineData(0, 0, 3, 3)>]
[<InlineData(2, 0, 3, -1)>]
[<InlineData(2, 0, 3, -10)>]
let ``Day15 Disc.tick works`` expectedPos pos count seconds =
    Assert.Equal(
        Disc(expectedPos, count),
        Disc(pos, count) |> Disc.tick seconds
    )

[<Fact>]
let ``Day15 findDropTime works`` () =
    let discs = [| Disc(4, 5); Disc(1, 2) |]
    Assert.Equal(5, discs |> findDropTime)

[<Fact>]
let ``Day15 Stars`` () =
    try
        Assert.Equal("148737", job1 ())
        Assert.Equal("2353212", job2 ())
    with :? System.NotImplementedException ->
        ()
