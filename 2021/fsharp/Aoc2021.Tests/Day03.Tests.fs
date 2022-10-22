module Day03.Tests

open Xunit

let report =
    [ "00100"
      "11110"
      "10110"
      "10111"
      "10101"
      "01111"
      "00111"
      "11100"
      "10000"
      "11001"
      "00010"
      "01010" ]

[<Fact>]
let ``Day03 power consumption works`` () =
    Assert.Equal({ Gamma = 22; Epsilon = 9 }, report |> PowerConsumtionRate.ofReport)

[<Fact>]
let ``Day03 life support works`` () =
    Assert.Equal(
        { OxygenGenerator = 23
          CO2Scrubber = 10 },
        report |> LifeSupportRate.ofReport
    )

[<Fact>]
let ``Day03 Stars`` () =
    Assert.Equal("1071734", job1 ())
    Assert.Equal("6124992", job2 ())
