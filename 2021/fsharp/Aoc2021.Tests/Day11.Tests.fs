module Day11.Tests

open Xunit

let input =
    [ "5483143223"
      "2745854711"
      "5264556173"
      "6141336146"
      "6357385478"
      "4167524645"
      "2176841721"
      "6882881134"
      "4846848554"
      "5283751526" ]

[<Fact>]
let ``Day11 totalFlashCount100 works`` () =
    Assert.Equal(1656, input |> totalFlashCount100)

[<Fact>]
let ``Day11 firstStepFullFlash works`` () =
    Assert.Equal(195, input |> firstStepFullFlash)

[<Fact>]
let ``Day11 Stars`` () =
    Assert.Equal("1725", job1 ())
    Assert.Equal("308", job2 ())
