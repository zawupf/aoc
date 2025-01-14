#load "Utils.fsx"
open Utils

type GuardId = GuardId of int
type Minute = Minute of int

type GuardEvent =
    | BeginShift of GuardId
    | FallAsleep of Minute
    | WakeUp of Minute

let parseEvent line =
    match line with
    | Regex @"Guard #(\d+) begins shift" [ Int id ] -> BeginShift <| GuardId id
    | Regex @"00:(\d{2})\] falls asleep" [ Int minute ] ->
        FallAsleep <| Minute minute
    | Regex @"00:(\d{2})\] wakes up" [ Int minute ] -> WakeUp <| Minute minute
    | _ -> failwithf "Invalid input: %s" line

let analyzeLog input =
    input
    |> Array.sort
    |> Array.map parseEvent
    |> Array.mapFold
        (fun (guard, (Minute asleep)) event ->
            match event with
            | BeginShift id -> None, (id, Minute 0)
            | FallAsleep asleep -> None, (guard, asleep)
            | WakeUp(Minute awake) ->
                Some [|
                    for minute = asleep to awake - 1 do
                        {| GuardId = guard; Minute = Minute minute |}
                |],
                (guard, Minute 0))
        (GuardId 0, Minute 0)
    |> fst
    |> Array.choose id
    |> Array.concat

let result (GuardId id) (Minute m) = id * m

let part1 input =
    let logData = input |> analyzeLog
    let guard = logData |> Array.countBy _.GuardId |> Array.maxBy snd |> fst

    let minute =
        logData
        |> Array.filter (fun x -> x.GuardId = guard)
        |> Array.countBy _.Minute
        |> Array.maxBy snd
        |> fst

    result guard minute

let part2 input =
    input
    |> analyzeLog
    |> Array.countBy id
    |> Array.maxBy snd
    |> fst
    |> fun x -> x.GuardId, x.Minute
    ||> result

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 65489 solution1
Test.run "Part 2" 3852 solution2

#load "_benchmark.fsx"
