module DayXY.Tests

open Xunit
open Utils
open DayXY

[<Fact>]
let ``DayXY Stars`` () =
    try
        Assert.Equal("", job1 ())
        Assert.Equal("", job2 ())
    with :? System.NotImplementedException ->
        ()
