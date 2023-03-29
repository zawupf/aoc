module Day11.Tests

open Xunit
open Utils
open Day11

// The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
// The second floor contains a hydrogen generator.
// The third floor contains a lithium generator.
// The fourth floor contains nothing relevant.

let mapping = [| "Hydrogen"; "Lithium" |]

let floors = [|
    [], [ "Hydrogen"; "Lithium" ]
    [ "Hydrogen" ], []
    [ "Lithium" ], []
    [], []
|]

[<Fact>]
let ``Day11 parsing works`` () =
    Assert.Equal(0x03uy, floors.[0] |> snd |> Bits.map mapping)
    Assert.Equal(0x00uy, floors.[0] |> fst |> Bits.map mapping)
    Assert.Equal(0x01uy, floors.[1] |> fst |> Bits.map mapping)
    Assert.Equal(0x02uy, floors.[2] |> fst |> Bits.map mapping)
    Assert.Equal(0x0300us, floors.[0] |> Bits.makeFloor mapping)
    Assert.Equal(0x0001us, floors.[1] |> Bits.makeFloor mapping)
    Assert.Equal(0x0002us, floors.[2] |> Bits.makeFloor mapping)
    Assert.Equal(0x0000us, floors.[3] |> Bits.makeFloor mapping)
    Assert.Equal(0x0000_0002_0001_0300UL, floors |> Bits.makeFloors mapping)
    Assert.Equal(0x0300us, floors |> Bits.makeFloors mapping |> Bits.floor 0)
    Assert.Equal(0x0001us, floors |> Bits.makeFloors mapping |> Bits.floor 1)
    Assert.Equal(0x0002us, floors |> Bits.makeFloors mapping |> Bits.floor 2)
    Assert.Equal(0x0000us, floors |> Bits.makeFloors mapping |> Bits.floor 3)
    Assert.Equal((1uy, 2uy), 0x0201us |> Bits.split)
    ()

[<Fact>]
let ``Day11 minStepCount works`` () =
    Assert.Equal(9, floors |> Bits.makeFloors mapping |> minStepCount)

[<Fact>]
let ``Day11 Stars`` () =
    try
        Assert.Equal("31", job1 ())
        Assert.Equal("55", job2 ())
    with :? System.NotImplementedException ->
        ()
