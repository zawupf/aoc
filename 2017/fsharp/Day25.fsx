#load "Utils.fsx"
open Utils

type Move =
    | Left
    | Right

type Rule = { Write: int; Move: Move; NextState: char }

type State = { Name: char; Rules: Rule[] }

let parseRule lines =
    // If the current value is 0: <- ignore
    //   - Write the value 1.
    //   - Move one slot to the right.
    //   - Continue with state B.
    let write =
        match lines |> Array.item 1 with
        | Regex @"Write the value (0|1)" [ Int write ] -> write
        | line -> failwithf "Invalid write: %s" line

    let move =
        match lines |> Array.item 2 with
        | Regex @"Move one slot to the (left|right)" [ move ] ->
            if move = "left" then Left else Right
        | line -> failwithf "Invalid move: %s" line

    let nextState =
        match lines |> Array.item 3 with
        | Regex @"Continue with state ([A-Z])" [ Char nextState ] -> nextState
        | line -> failwithf "Invalid next state: %s" line

    { Write = write; Move = move; NextState = nextState }

let parseState lines =
    // In state A:
    let name =
        match lines |> Array.item 0 with
        | Regex @"In state ([A-Z])" [ Char name ] -> name
        | line -> failwithf "Invalid name: %s" line

    let rules =
        lines |> Array.skip 1 |> Array.chunkBySize 4 |> Array.map parseRule

    { Name = name; Rules = rules }

let parse input =
    let sections = input |> String.toSections

    // Begin in state A.
    // Perform a diagnostic checksum after 6 steps.
    let header = sections[0] |> String.toLines

    let startState =
        match header[0] with
        | Regex @"Begin in state ([A-Z])" [ Char startState ] -> startState
        | line -> failwithf "Invalid start state: %s" line

    let steps =
        match header[1] with
        | Regex @"Perform a diagnostic checksum after (\d+) steps." [ Int steps ] ->
            steps
        | line -> failwithf "Invalid steps: %s" line

    let states =
        sections
        |> Array.skip 1
        |> Array.map (String.toLines >> parseState)
        |> Array.map (fun state -> state.Name, state.Rules)
        |> Dictionary.ofSeq

    steps, startState, states

let checksum (steps, state, states) =
    let tape = HashSet()

    let rec loop steps state index =
        match steps with
        | 0 -> tape.Count
        | _ ->
            let rules = states |> Dictionary.get state
            let value = tape |> HashSet.contains index |> Bool.toInt
            let rule = rules |> Array.item value

            match rule.Write with
            | 0 -> tape |> HashSet.remove index |> ignore
            | 1 -> tape |> HashSet.add index |> ignore
            | write -> failwithf "Invalid write: %d" write

            loop
                (steps - 1)
                rule.NextState
                (if rule.Move = Left then index - 1 else index + 1)

    loop steps state 0

let part1 input =
    let foo = input |> parse
    foo |> checksum

let part2 input = input |> notImplemented ()

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.
"""
    |]
    |> Array.map String.trim

Test.run "Test 1" 3 (fun () -> part1 testInput[0])

Test.run "Part 1" 2526 solution1

#load "_benchmark.fsx"
