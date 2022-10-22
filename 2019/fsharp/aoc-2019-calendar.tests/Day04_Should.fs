module Tests04

open Xunit
open Day04

[<Fact>]
let ``Day04 isValidAdjacentDigits works`` () =
    Assert.True(isValidAdjacentDigits 111111)
    Assert.True(isValidAdjacentDigits 223450)
    Assert.False(isValidAdjacentDigits 123789)

[<Fact>]
let ``Day04 isValidAdjacentDigits2 works`` () =
    Assert.False(isValidAdjacentDigits2 111111)
    Assert.True(isValidAdjacentDigits2 223450)
    Assert.False(isValidAdjacentDigits2 123789)

    Assert.True(isValidAdjacentDigits2 112233)
    Assert.False(isValidAdjacentDigits2 123444)
    Assert.True(isValidAdjacentDigits2 111122)

[<Fact>]
let ``Day04 isValidNeverDecrease works`` () =
    Assert.True(isValidNeverDecrease 111111)
    Assert.False(isValidNeverDecrease 223450)
    Assert.True(isValidNeverDecrease 123789)

[<Fact>]
let ``Day04 Stars`` () =
    Assert.Equal("2050", job1 ())
    Assert.Equal("1390", job2 ())
    ()
