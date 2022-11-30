module Day02.Tests

open Xunit
open Day02

let commands =
    [ "forward 5"; "down 5"; "forward 8"; "up 3"; "down 8"; "forward 2" ]
    |> parseCommands

let submarine =
    { HorizontalPosition = 0
      Depth = 0
      Aim = 0 }

[<Fact>]
let ``Day02 move part 1 works`` () =
    Assert.Equal(
        { HorizontalPosition = 15
          Depth = 10
          Aim = 0 },
        commands |> moveFirstTry submarine
    )

[<Fact>]
let ``Day02 move part 2 works`` () =
    Assert.Equal(
        { HorizontalPosition = 15
          Depth = 60
          Aim = 0 },
        { (commands |> moveSecondTry submarine) with
            Aim = 0 }
    )

[<Fact>]
let ``Day02 Stars`` () =
    Assert.Equal("1507611", job1 ())
    Assert.Equal("1880593125", job2 ())
