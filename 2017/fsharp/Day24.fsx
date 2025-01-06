#load "Utils.fsx"
open Utils

type PartialPort = { Type: int; TotalStrength: int; Length: int }
type Port = { Types: int * int; Strength: int }

module Port =
    let isMatch partial port =
        match port.Types with
        | a, b -> a = partial.Type || b = partial.Type

    let connect partial port =
        let strength = partial.TotalStrength + port.Strength
        let length = partial.Length + 1

        match port.Types with
        | a, b when a = partial.Type -> {
            Type = b
            TotalStrength = strength
            Length = length
          }
        | a, b when b = partial.Type -> {
            Type = a
            TotalStrength = strength
            Length = length
          }
        | _ -> failwithf "Invalid port w/o type: %d" partial.Type

let parsePort line =
    match line |> String.parseInts '/' with
    | [| a; b |] -> { Types = a, b; Strength = a + b }
    | _ -> failwithf "Invalid input: %s" line

let parse input = input |> Array.map parsePort |> Array.toList

let connectTo ports partial =
    ports
    |> List.choose (fun p ->
        match p |> Port.isMatch partial with
        | true -> Some(Port.connect partial p, List.except [ p ] ports)
        | false -> None)

let strongestBridge ports =
    let rec loop result list =
        match list with
        | [] -> result
        | (connected, remaining) :: list ->
            loop
                (max result connected.TotalStrength)
                (connectTo remaining connected @ list)

    loop 0 [ { Type = 0; TotalStrength = 0; Length = 0 }, ports ]

let strongestLongestBridge ports =
    let rec loop result list =
        match list with
        | [] -> result |> snd
        | (connected, remaining) :: list ->
            loop
                (max result (connected.Length, connected.TotalStrength))
                (connectTo remaining connected @ list)

    loop (0, 0) [ { Type = 0; TotalStrength = 0; Length = 0 }, ports ]

let part1 input = input |> parse |> strongestBridge

let part2 input = input |> parse |> strongestLongestBridge

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 31 (fun () -> part1 testInput[0])
Test.run "Test 2" 19 (fun () -> part2 testInput[0])

Test.run "Part 1" 1511 solution1 // too low: 1159
Test.run "Part 2" 1471 solution2

#load "_benchmark.fsx"
