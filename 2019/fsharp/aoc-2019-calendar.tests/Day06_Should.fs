module Tests06

open Xunit
open Day06

module Data =
    let orbits =
        [ "COM)B"
          "B)C"
          "C)D"
          "D)E"
          "E)F"
          "B)G"
          "G)H"
          "D)I"
          "E)J"
          "J)K"
          "K)L" ]

    let orbits2 =
        [ "COM)B"
          "B)C"
          "C)D"
          "D)E"
          "E)F"
          "B)G"
          "G)H"
          "D)I"
          "E)J"
          "J)K"
          "K)L"
          "K)YOU"
          "I)SAN" ]

[<Fact>]
let ``Day06 parse works`` () =
    let orbits = parse Data.orbits
    Assert.Equal("COM", planetOf "B" orbits)
    Assert.Equal("E", planetOf "J" orbits)
    Assert.Equal("E", planetOf "F" orbits)

    Assert.Equal<List<_>>([], orbits |> allPlanetsOf "COM")
    Assert.Equal<List<_>>([ "COM"; "B"; "C"; "D"; "E"; "J"; "K" ], orbits |> allPlanetsOf "L")

[<Fact>]
let ``Day06 checksum works`` () =
    let orbits = parse Data.orbits
    Assert.Equal(42, orbits |> checksum)

[<Fact>]
let ``Day06 minimalTransferCount works`` () =
    let orbits = parse Data.orbits2
    Assert.Equal(4, orbits |> minimalTransferCount)

[<Fact>]
let ``Day06 Stars`` () =
    Assert.Equal("247089", job1 ())
    Assert.Equal("442", job2 ())
    ()
