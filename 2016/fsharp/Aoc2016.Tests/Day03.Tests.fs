module Day03.Tests

open Xunit
open Utils
open Day03

[<Theory>]
[<InlineData(false, "5 10 25")>]
[<InlineData(true, "3 4 5")>]
let ``Day03 isMaybeTriangle works`` expected triangle =
    Assert.Equal(
        expected,
        triangle |> Triangle.parse |> Triangle.isMaybeTriangle
    )

[<Fact>]
let ``Day03 Stars`` () =
    try
        Assert.Equal("869", job1 ())
        Assert.Equal("1544", job2 ())
    with :? System.NotImplementedException ->
        ()
