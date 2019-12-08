module Day05

open System
open System.IO
open Day02.Computer

let job1 prefix =
    let source = File.ReadAllText(prefix + "inputs/Day05.txt")

    let context: Context = compile source
    context.input.Enqueue(1)
    runSilent context |> ignore
    context.output.ToArray()
    |> Array.last
    |> string

let job2 prefix =
    let source = File.ReadAllText(prefix + "inputs/Day05.txt")

    let context: Context = compile source
    context.input.Enqueue(5)
    runSilent context |> ignore
    context.output.Dequeue() |> string
