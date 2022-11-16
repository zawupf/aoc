module Day06.Tests

open Xunit
open Utils
open Day06

let input =
    """
    eedadn
    drvtee
    eandsr
    raavrd
    atevrs
    tsrnev
    sdttsa
    rasrtv
    nssdts
    ntnada
    svetve
    tesnvt
    vntsnd
    vrdear
    dvrsen
    enarar
    """
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim

[<Fact>]
let ``Day06 corrected works`` () =
    Assert.Equal("easter", input |> corrected Seq.maxBy)
    Assert.Equal("advent", input |> corrected Seq.minBy)

[<Fact>]
let ``Day06 Stars`` () =
    try
        Assert.Equal("qoclwvah", job1 ())
        Assert.Equal("ryrgviuv", job2 ())
    with :? System.NotImplementedException ->
        ()
