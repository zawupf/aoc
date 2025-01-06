#load "Utils.fsx"
open Utils

type Layer = { Depth: int; Range: int }

let parse line =
    match line |> String.parseInts ": " with
    | [| depth; range |] -> { Depth = depth; Range = range }
    | _ -> failwithf "Invalid input: %s" line

let scannerPosition time layer =
    let range = layer.Range - 1
    let period = range * 2
    let position = time % period

    if position > range then range - (position - range) else position

let findDelayToPassFirewall layers =
    let rec findDelay delay =
        if
            layers
            |> Array.exists (fun layer ->
                scannerPosition (layer.Depth + delay) layer = 0)
        then
            findDelay (delay + 1)
        else
            delay

    findDelay 0

let part1 input =
    input
    |> Array.map parse
    |> Array.filter (fun layer -> scannerPosition layer.Depth layer = 0)
    |> Array.sumBy (fun layer -> layer.Depth * layer.Range)

let part2 input = input |> Array.map parse |> findDelayToPassFirewall

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
0: 3
1: 2
4: 4
6: 4
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 24 (fun () -> part1 testInput[0])
Test.run "Test 2" 10 (fun () -> part2 testInput[0])

Test.run "Part 1" 1624 solution1
Test.run "Part 2" 3923436 solution2

#load "_benchmark.fsx"
