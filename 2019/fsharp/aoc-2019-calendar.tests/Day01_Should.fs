module Tests01

open Xunit
open Day01

[<Theory>]
[<InlineData(12, 2)>]
[<InlineData(14, 2)>]
[<InlineData(1969, 654)>]
[<InlineData(100756, 33583)>]
let ``Day01 fuel works`` mass expectedFuel =
    Assert.Equal(expectedFuel, fuel mass)

[<Fact>]
let ``Day01 Stars`` () =
    Assert.Equal("3421505", job1 ())
    Assert.Equal("5129386", job2 ())
