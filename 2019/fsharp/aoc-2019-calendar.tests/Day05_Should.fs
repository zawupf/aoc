module Tests05

open System
open Xunit
open Day05
open Day02.Computer

[<Theory>]
[<InlineData("1002,4,3,4,33", "1002,4,3,4,99")>]
let ``Day05 immediate works`` (code: string) expected =
    let context = compile code
    runSilent context |> ignore
    let result = String.Join(',', context.memory |> Array.map string)
    Assert.Equal(expected, result)

[<Theory>]
[<InlineData(0)>]
[<InlineData(1)>]
[<InlineData(2)>]
let ``Day05 Computer in/out works`` (value) =
    let context = compile "3,0,4,0,99"
    context.input.Enqueue(value)
    let event = runSilent context
    Assert.Equal(Halted, event)
    Assert.Equal(value, context.output.Dequeue())

let exec source input =
    let context = compile source
    context.input.Enqueue(input)
    runSilent context |> ignore
    context.output.Dequeue()

[<Fact>]
let ``Day05 equals works``() =
    Assert.Equal(1, exec "3,9,8,9,10,9,4,9,99,-1,8" 8)
    Assert.Equal(0, exec "3,9,8,9,10,9,4,9,99,-1,8" 9)

    Assert.Equal(1, exec "3,3,1108,-1,8,3,4,3,99" 8)
    Assert.Equal(0, exec "3,3,1108,-1,8,3,4,3,99" 9)

[<Fact>]
let ``Day05 lessThan works``() =
    Assert.Equal(0, exec "3,9,7,9,10,9,4,9,99,-1,8" 8)
    Assert.Equal(1, exec "3,9,7,9,10,9,4,9,99,-1,8" 7)

    Assert.Equal(0, exec "3,3,1107,-1,8,3,4,3,99" 8)
    Assert.Equal(1, exec "3,3,1107,-1,8,3,4,3,99" 7)

[<Fact>]
let ``Day05 jump works``() =
    Assert.Equal(0, exec "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9" 0)
    Assert.Equal(1, exec "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9" -1)

    Assert.Equal(0, exec "3,3,1105,-1,9,1101,0,0,12,4,12,99,1" 0)
    Assert.Equal(1, exec "3,3,1105,-1,9,1101,0,0,12,4,12,99,1" -1)

[<Fact>]
let ``Day05 complex code works``() =
    let code =
        "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"
    Assert.Equal(999, exec code 7)
    Assert.Equal(1000, exec code 8)
    Assert.Equal(1001, exec code 9)

[<Fact>]
let ``Day05 Stars``() =
    Assert.Equal("9025675", job1 "../../../../")
    Assert.Equal("11981754", job2 "../../../../")
