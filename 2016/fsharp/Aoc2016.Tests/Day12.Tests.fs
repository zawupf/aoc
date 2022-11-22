module Day12.Tests

open Xunit
open Utils
open Day12

[<Fact>]
let ``Day12 computer works`` () =
    let input = [| "cpy 41 a"; "inc a"; "inc a"; "dec a"; "jnz a 2"; "dec a" |]

    Assert.Equal(
        42<value>,
        Computer.empty |> Computer.load input |> Computer.run |> Computer.get RA
    )

[<Fact>]
let ``Day12 Stars`` () =
    try
        Assert.Equal("318020", job1 ())
        Assert.Equal("9227674", job2 ())
    with :? System.NotImplementedException ->
        ()
