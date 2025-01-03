#load "Utils.fsx"
open Utils

type HexDir =
    | N
    | NE
    | SE
    | S
    | SW
    | NW

let eliminations = [|
    (NW, NE), Some N
    (SE, SW), Some S
    (N, SE), Some NE
    (S, NW), Some SW
    (SW, N), Some NW
    (NE, S), Some SE
    (N, S), None
    (NE, SW), None
    (SE, NW), None
|]

let parse input =
    input
    |> String.split ","
    |> Array.map (function
        | "n" -> N
        | "ne" -> NE
        | "se" -> SE
        | "s" -> S
        | "sw" -> SW
        | "nw" -> NW
        | _ -> failwith "Invalid input")

let eliminate map =
    let rec loop (map: Dictionary<HexDir, int>) =
        eliminations
        |> Array.map (fun ((a, b), result) ->
            match min map[a] map[b] with
            | 0 -> false
            | count ->
                map[a] <- map[a] - count
                map[b] <- map[b] - count
                result |> Option.iter (fun dir -> map[dir] <- map[dir] + count)
                true)
        |> Array.exists id
        |> function
            | true -> loop map
            | false -> map

    loop map

let fillMap map =
    [ N; NE; SE; S; SW; NW ]
    |> List.iter (fun dir ->
        map
        |> Dictionary.update dir (function
            | Some count -> count
            | None -> 0)
        |> ignore)

    map

let finalSteps dirs =
    dirs
    |> Array.countBy id
    |> Dictionary.ofSeq
    |> fillMap
    |> eliminate
    |> Dictionary.values
    |> Seq.sum

let furthestSteps dirs =
    let map = Dictionary<HexDir, int>() |> fillMap

    dirs
    |> Array.map (fun dir ->
        map
        |> Dictionary.update dir (function
            | Some count -> count + 1
            | None -> unreachable ())
        |> eliminate
        |> Dictionary.values
        |> Seq.sum)
    |> Array.max

let part1 input = input |> parse |> finalSteps

let part2 input = input |> parse |> furthestSteps

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput = [|
    "ne,ne,ne", 3
    "ne,ne,sw,sw", 0
    "ne,ne,s,s", 2
    "se,sw,se,sw,sw", 3
|]

for input, expected in testInput do
    Test.run $"Test 1 ({input})" expected (fun () -> part1 input)

Test.run "Part 1" 824 solution1
Test.run "Part 2" 1548 solution2

#load "_benchmark.fsx"
