module Day09

open System
open Utils
open Computer

let private test context =
    context.input.Enqueue(1L)
    runSilent context |> ignore

    match context.output.Count with
    | 1 -> context.output.Dequeue()
    | _ ->
        printfn "Errors: %A" (context.output.ToArray())
        failwith "BOOST errors"

let private boost context =
    context.input.Enqueue(2L)
    runSilent context |> ignore
    context.output.Dequeue()

let job1 () =
    readInputText "09" |> compile |> test |> string

let job2 () =
    readInputText "09" |> compile |> boost |> string
