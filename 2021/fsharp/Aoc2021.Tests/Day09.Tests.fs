module Day09.Tests

open Xunit

let input = [ "2199943210"; "3987894921"; "9856789892"; "8767896789"; "9899965678" ]

[<Fact>]
let ``Day09 riskLevelSum works`` () = Assert.Equal(15, input |> riskLevelSum)

[<Fact>]
let ``Day09 basinProduct works`` () =
    Assert.Equal(1134, input |> basinProduct)

[<Fact>]
let ``Day09 Stars`` () =
    Assert.Equal("448", job1 ())
    Assert.Equal("1417248", job2 ())
