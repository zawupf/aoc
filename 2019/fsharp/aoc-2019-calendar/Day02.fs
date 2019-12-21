module Day02

open Utils
open Computer

let job1() =
    let code = readInputText "02"

    let context: Context = patch (compile code) 12 2
    runSilent context |> ignore
    context.memory.[0] |> string

let job2() =
    let code = readInputText "02"

    let originalContext = compile code

    let exec noun verb =
        let context = patch originalContext noun verb
        runSilent context |> ignore
        context.memory.[0]

    seq {
        for noun in 0 .. 99 do
            for verb in 0 .. 99 -> noun, verb
    }
    |> Seq.map (fun (noun, verb) -> (exec noun verb), noun * 100 + verb)
    |> Seq.filter (fun (exitCode, _) -> exitCode = 19690720)
    |> Seq.head
    |> snd
    |> string
