module Day06.Tests

open Xunit
open Utils
open Day06

[<Fact>]
let ``Day06 actions work`` () =
    let grid = newGrid ()

    let processAction input =
        input |> parseAction |> handleAction1 grid |> Array.sum

    Assert.Equal(0, grid |> Array.sum)
    Assert.Equal(1_000_000, "turn on 0,0 through 999,999" |> processAction)
    Assert.Equal(1_000_000 - 1_000, "toggle 0,0 through 999,0" |> processAction)

    Assert.Equal(
        1_000_000 - 1_000 - 4,
        "turn off 499,499 through 500,500" |> processAction
    )

    Assert.Equal(1_000_000 - 4, "toggle 0,0 through 999,0" |> processAction)

[<Fact>]
let ``Day06 actions 2 work`` () =
    let grid = newGrid ()

    let processAction input =
        input |> parseAction |> handleAction2 grid |> Array.sum

    Assert.Equal(0, grid |> Array.sum)
    Assert.Equal(1_000_000, "turn on 0,0 through 999,999" |> processAction)
    Assert.Equal(1_000_000 + 2_000, "toggle 0,0 through 999,0" |> processAction)

    Assert.Equal(
        1_000_000 + 2_000 - 4,
        "turn off 499,499 through 500,500" |> processAction
    )

    Assert.Equal(
        1_000_000 + 4_000 - 4,
        "toggle 0,0 through 999,0" |> processAction
    )

[<Fact>]
let ``Day06 Stars`` () =
    try
        Assert.Equal("543903", job1 ())
        Assert.Equal("14687245", job2 ())
    with :? System.NotImplementedException ->
        ()
