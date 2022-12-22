module Day20.Tests

open Xunit
open Utils
open Day20

let input =
    """
    1
    2
    -3
    3
    -2
    0
    4
    """
    |> String.toLines

[<Theory>]
[<InlineData("1, 2, -3, 3, -2, 0, 4", 0, 0<nid>)>]
[<InlineData("2, 1, -3, 3, -2, 0, 4", 1, 1<nid>)>]
[<InlineData("1, -3, 2, 3, -2, 0, 4", 2, 0<nid>)>]
[<InlineData("1, 2, 3, -2, -3, 0, 4", 3, 0<nid>)>]
[<InlineData("1, 2, -2, -3, 0, 3, 4", 4, 0<nid>)>]
[<InlineData("1, 2, -3, 0, 3, 4, -2", 5, 0<nid>)>]
[<InlineData("1, 2, -3, 0, 3, 4, -2", 6, 0<nid>)>]
[<InlineData("1, 2, -3, 4, 0, 3, -2", 7, 0<nid>)>]
let ``Day20 mix_n works`` expected n id' =
    Assert.Equal(expected, input |> parse |> mix_n n |> dump id')

[<Fact>]
let ``Day20 groveCoordinates works`` () =
    Assert.Equal<int64 list>(
        [ 4L; -3L; 2L ],
        input |> parse |> groveCoordinates 1L 1
    )

    Assert.Equal<int64 list>(
        [ 811589153L; 2434767459L; -1623178306L ],
        input |> parse |> groveCoordinates 811589153L 10
    )

[<Fact>]
let ``Day20 groveCoordinatesSum works`` () =
    Assert.Equal(3L, input |> parse |> groveCoordinatesSum 1L 1)

    Assert.Equal(
        1623178306L,
        input |> parse |> groveCoordinatesSum 811589153L 10
    )

[<Fact>]
let ``Day20 Stars`` () =
    try
        Assert.Equal("4914", job1 ())
        Assert.Equal("7973051839072", job2 ())
        ()
    with :? System.NotImplementedException ->
        ()
