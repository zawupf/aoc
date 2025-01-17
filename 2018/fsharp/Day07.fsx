#load "Utils.fsx"
open Utils

let parseDep line =
    let linePattern = @"Step (\w) must be finished before step (\w) can begin."

    match line with
    | Regex linePattern [ Char first; Char second ] -> first, second
    | _ -> failwithf "Invalid input: %s" line

let END_MARKER = '0'

let parseDeps lines =
    let deps = lines |> Array.map parseDep |> Array.toList
    let lastSteps = deps |> List.map snd |> List.except (deps |> List.map fst)
    List.append deps (lastSteps |> List.map (fun step -> step, END_MARKER))

let part1 input =
    let nextStep deps =
        let next =
            deps
            |> List.map fst
            |> List.except (deps |> List.map snd)
            |> List.min

        let deps = deps |> List.filter (fst >> (<>) next)
        next, deps

    let rec loop result deps =
        match deps |> nextStep with
        | step, [] -> step :: result |> List.rev |> String.ofChars
        | step, deps -> loop (step :: result) deps

    input |> parseDeps |> loop []

let part2 input =
    let nextWorkers currentTime workers deps =
        let nextSteps =
            deps
            |> List.map fst
            |> List.except (List.map snd workers @ List.map snd deps)
            |> List.sort
            |> List.truncate (5 - List.length workers)

        let workers =
            workers
            @ (nextSteps
               |> List.map (fun step ->
                   currentTime + 61 + int (step - 'A'), step))
            |> List.sort

        workers

    let rec loop currentTime workers deps =
        match deps |> nextWorkers currentTime workers with
        | [] -> currentTime
        | workers ->
            let time, _ = workers |> List.head

            let completedSteps, remainingWorkers =
                workers
                |> List.partition (fst >> (=) time)
                |> fun (completed, remaining) ->
                    completed |> List.map snd, remaining

            let remainingDeps =
                deps
                |> List.filter (fun (a, _) ->
                    completedSteps |> List.contains a |> not)

            loop time remainingWorkers remainingDeps

    input |> parseDeps |> loop 0 []

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" "PFKQWJSVUXEMNIHGTYDOZACRLB" solution1
Test.run "Part 2" 864 solution2

#load "_benchmark.fsx"
