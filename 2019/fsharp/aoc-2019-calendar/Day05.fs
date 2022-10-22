module Day05

open Utils
open Computer

let job1 () =
    let source = readInputText "05"

    let context: Context = compile source
    context.input.Enqueue(1L)
    runSilent context |> ignore
    context.output.ToArray() |> Array.last |> string

let job2 () =
    let source = readInputText "05"

    let context: Context = compile source
    context.input.Enqueue(5L)
    runSilent context |> ignore
    context.output.Dequeue() |> string
