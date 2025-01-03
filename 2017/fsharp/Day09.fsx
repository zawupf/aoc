#load "Utils.fsx"
open Utils

type Context =
    | Group of level: int
    | Garbage of count: int
    | Ignore

type State =
    | State of context: Context * previous: State
    | Done

type Event =
    | GroupClosed of level: int
    | GarbageRemoved of count: int

let processStream input =
    input
    |> Seq.mapFold
        (fun state c ->
            let context, previous =
                match state with
                | State(context, previous) -> context, previous
                | Done -> failwith "Unexpected state"

            match context with
            | Group level ->
                match c with
                | '{' -> None, State(Group(level + 1), state)
                | '}' -> Some(GroupClosed level), previous
                | '<' -> None, State(Garbage 0, state)
                | _ -> None, state
            | Garbage count ->
                match c with
                | '!' -> None, State(Ignore, state)
                | '>' -> Some(GarbageRemoved count), previous
                | _ -> None, State(Garbage(count + 1), previous)
            | Ignore -> None, previous)
        (State(Group 0, Done))
    |> fst
    |> Seq.choose id

let totalScore input =
    input
    |> processStream
    |> Seq.sumBy (function
        | GroupClosed level -> level
        | _ -> 0)

let removedGarbage input =
    input
    |> processStream
    |> Seq.sumBy (function
        | GarbageRemoved count -> count
        | _ -> 0)

let part1 input = input |> totalScore

let part2 input = input |> removedGarbage

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testData = [|
    "{}", 1
    "{{{}}}", 6
    "{{},{}}", 5
    "{{{},{},{{}}}}", 16
    "{<a>,<a>,<a>,<a>}", 1
    "{{<ab>},{<ab>},{<ab>},{<ab>}}", 9
    "{{<!!>},{<!!>},{<!!>},{<!!>}}", 9
    "{{<a!>},{<a!>},{<a!>},{<ab>}}", 3
|]

for input, expected in testData do
    Test.run $"Test 1 ({input})" expected (fun () -> part1 input)

let testData2 = [|
    "<>", 0
    "<random characters>", 17
    "<<<<>", 3
    "<{!>}>", 2
    "<!!>", 0
    "<!!!>>", 0
    "<{o\"i!a,<{i<a>", 10
|]

for input, expected in testData2 do
    Test.run $"Test 2 ({input})" expected (fun () -> part2 input)

Test.run "Part 1" 16021 solution1
Test.run "Part 2" 7685 solution2

#load "_benchmark.fsx"
