module Day00.Tests

open Xunit
open Day00

[<Theory>]
[<InlineData(12, 2)>]
[<InlineData(14, 2)>]
[<InlineData(1969, 654)>]
[<InlineData(100756, 33583)>]
let ``Day00 fuel works`` mass expectedFuel = Assert.Equal(expectedFuel, fuel mass)

[<Fact>]
let ``Day00 Stars`` () =
    Assert.Equal("3421505", job1 ())
    Assert.Equal("5129386", job2 ())
