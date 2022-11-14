module Day01.Tests

open Xunit
open Utils
open Day01

[<Theory>]
[<InlineData(5, "R2, L3")>]
[<InlineData(2, "R2, R2, R2")>]
[<InlineData(12, "R5, L5, R5, R3")>]
let ``Day01 part 1 works`` dist steps =
    Assert.Equal(dist, steps |> parse |> walk1 |> distance)

[<Theory>]
[<InlineData(4, "R8, R4, R4, R8")>]
let ``Day01 part 2 works`` dist steps =
    Assert.Equal(dist, steps |> parse |> walk2 |> distance)

[<Fact>]
let ``Day01 Stars`` () =
    try
        Assert.Equal("279", job1 ())
        Assert.Equal("163", job2 ())
    with :? System.NotImplementedException ->
        ()
