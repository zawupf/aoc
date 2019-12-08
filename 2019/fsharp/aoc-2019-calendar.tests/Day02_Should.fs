module Tests02

open System
open Xunit
open Day02

[<Theory>]
[<InlineData("1,0,0,0,99", "2,0,0,0,99")>]
[<InlineData("2,3,0,3,99", "2,3,0,6,99")>]
[<InlineData("2,4,4,5,99,0", "2,4,4,5,99,9801")>]
[<InlineData("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")>]
let ``Day02 exec works`` (code: string) expected =
    let context = Computer.compile code
    Computer.runSilent context |> ignore
    let result = String.Join(',', context.memory |> Array.map string)
    Assert.Equal(expected, result)

[<Fact>]
let ``Day02 Stars``() =
    Assert.Equal("3765464", job1 "../../../../")
    Assert.Equal("7610", job2 "../../../../")
