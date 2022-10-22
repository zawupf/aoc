module Day07

open System
open Utils
open Computer

let private makeAmplifier source phase =
    let context = compile source
    context.input.Enqueue(phase)
    context

let private pipe input context =
    let value =
        match input with
        | Some value -> value
        | None -> failwith "Input required"

    context.input.Enqueue(value)

    match run context with
    | Event.Output result -> Some result
    | _ -> input

let thrusterSignal source phases =
    phases
    |> List.map (makeAmplifier source)
    |> List.fold pipe (Some 0L)
    |> Option.get

let thrusterSignalWithFeedback source phases =
    let amplifiers = phases |> List.map (makeAmplifier source)

    let rec pipeline input =
        let output = amplifiers |> List.fold pipe input

        if amplifiers |> List.last |> isHalted then
            output
        else
            pipeline output

    Some 0L |> pipeline |> Option.get

let maxThrusterSignal source =
    [ 0L; 1L; 2L; 3L; 4L ]
    |> permutations
    |> Seq.map (thrusterSignal source)
    |> Seq.max

let maxThrusterSignalWithFeedback source =
    [ 5L; 6L; 7L; 8L; 9L ]
    |> permutations
    |> Seq.map (thrusterSignalWithFeedback source)
    |> Seq.max

let job1 () =
    readInputText "07" |> maxThrusterSignal |> string

let job2 () =
    readInputText "07" |> maxThrusterSignalWithFeedback |> string
