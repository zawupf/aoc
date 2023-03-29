module Day10.Tests

open Xunit
open Utils
open Day10

[<Fact>]
let ``Day10 factory works`` () =
    let factory =
        [|
            "value 5 goes to bot 2"
            "bot 2 gives low to bot 1 and high to bot 0"
            "value 3 goes to bot 1"
            "bot 1 gives low to output 1 and high to bot 0"
            "bot 0 gives low to output 2 and high to output 0"
            "value 2 goes to bot 2"
        |]
        |> Factory.init

    Assert.Equal(3, factory.Bots |> Map.count)

    Assert.Equal(
        2,
        factory |> Factory.findBot (fun bot -> bot.Chips = [ 2; 5 ])
    )

    Assert.Equal(5 * 2 * 3, factory |> Factory.getOutputProduct)

[<Fact>]
let ``Day10 Stars`` () =
    try
        Assert.Equal("113", job1 ())
        Assert.Equal("12803", job2 ())
    with :? System.NotImplementedException ->
        ()
