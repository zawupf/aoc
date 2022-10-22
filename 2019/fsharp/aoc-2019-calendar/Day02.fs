module Day02

open Utils
open Computer

let job1 () =
    let code = readInputText "02"

    let context: Context = patch (compile code) 12L 2L
    runSilent context |> ignore
    context.memory.[0] |> string

let job2 () =
    let code = readInputText "02"

    let originalContext = compile code

    let exec noun verb =
        let context = patch originalContext noun verb
        runSilent context |> ignore
        context.memory.[0]

    seq {
        for noun in 0L .. 99L do
            for verb in 0L .. 99L -> noun, verb
    }
    |> Seq.map (fun (noun, verb) -> (exec noun verb), noun * 100L + verb)
    |> Seq.filter (fun (exitCode, _) -> exitCode = 19690720L)
    |> Seq.head
    |> snd
    |> string
