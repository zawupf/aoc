module Day17.Tests

open Xunit
open Utils
open Day17

[<Fact>]
let ``Day17 chamberHeightAfter works`` () =
    Assert.Equal(
        3068L,
        chamberHeightAfter 2022L ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    )

// Assert.Equal(
//     1514285714288L,
//     chamberHeightAfter
//         10000000L
//         // 1000000000000L
//         ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
// )

[<Fact>]
let ``Day17 Stars`` () =
    try
        Assert.Equal("3067", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
