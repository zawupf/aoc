module TestsComputer

open System
open Xunit
open Computer
open Utils

[<Theory>]
[<InlineData("1,0,0,0,99", "2,0,0,0,99")>]
[<InlineData("2,3,0,3,99", "2,3,0,6,99")>]
[<InlineData("2,4,4,5,99,0", "2,4,4,5,99,9801")>]
[<InlineData("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")>]
let ``Computer Day02 exec works`` (code: string) expected =
    let context = compile code
    runSilent context |> ignore
    let result = String.join "," (context.memory |> Array.map string)
    Assert.Equal(expected, result)

[<Theory>]
[<InlineData("1002,4,3,4,33", "1002,4,3,4,99")>]
let ``Computer Day05 immediate works`` (code: string) expected =
    let context = compile code
    runSilent context |> ignore
    let result = String.join "," (context.memory |> Array.map string)
    Assert.Equal(expected, result)

[<Theory>]
[<InlineData(0)>]
[<InlineData(1)>]
[<InlineData(2)>]
let ``Computer Day05 in/out works`` (value) =
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
let ``Computer Day05 equals works``() =
    Assert.Equal(1L, exec "3,9,8,9,10,9,4,9,99,-1,8" 8L)
    Assert.Equal(0L, exec "3,9,8,9,10,9,4,9,99,-1,8" 9L)

    Assert.Equal(1L, exec "3,3,1108,-1,8,3,4,3,99" 8L)
    Assert.Equal(0L, exec "3,3,1108,-1,8,3,4,3,99" 9L)

[<Fact>]
let ``Computer Day05 lessThan works``() =
    Assert.Equal(0L, exec "3,9,7,9,10,9,4,9,99,-1,8" 8L)
    Assert.Equal(1L, exec "3,9,7,9,10,9,4,9,99,-1,8" 7L)

    Assert.Equal(0L, exec "3,3,1107,-1,8,3,4,3,99" 8L)
    Assert.Equal(1L, exec "3,3,1107,-1,8,3,4,3,99" 7L)

[<Fact>]
let ``Computer Day05 jump works``() =
    Assert.Equal(0L, exec "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9" 0L)
    Assert.Equal(1L, exec "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9" -1L)

    Assert.Equal(0L, exec "3,3,1105,-1,9,1101,0,0,12,4,12,99,1" 0L)
    Assert.Equal(1L, exec "3,3,1105,-1,9,1101,0,0,12,4,12,99,1" -1L)

[<Fact>]
let ``Computer Day05 complex code works``() =
    let code =
        "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"
    Assert.Equal(999L, exec code 7L)
    Assert.Equal(1000L, exec code 8L)
    Assert.Equal(1001L, exec code 9L)

let exec2 source =
    let context = compile source
    runSilent context |> ignore
    let result = context.output.ToArray() |> Array.map string
    String.join "," result

[<Fact>]
let ``Computer Day09 latest features work``() =
    let code = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99"
    Assert.Equal(code, exec2 code)
    Assert.Equal(16, (exec2 "1102,34915192,34915192,7,4,7,99,0").Length)
    Assert.Equal("1125899906842624", exec2 "104,1125899906842624,99")
