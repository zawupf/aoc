module Day04

open Utils

type Range = int * int

module Range =
    let init a b = if a < b then (a, b) else (b, a)

    let length (a, b) = b - a + 1

    let isOneFullyContained (a, b) (c, d) = c >= a && d <= b || a >= c && b <= d

    let isOverlapping (a, b) (c, d) = c >= a && c <= b || a >= c && a <= d

    let parse =
        function
        | Regex @"^(\d+)-(\d+)$" [ Int a; Int b ] -> init a b
        | range -> failwith $"Invalid range: %s{range}"

    let parsePair line =
        line
        |> String.split ','
        |> Array.map parse
        |> function
            | [| range1; range2 |] -> range1, range2
            | _ -> failwith $"Invalid range pair: %s{line}"

let countFullyContainedPairs pairs =
    pairs
    |> List.filter (fun pair -> pair ||> Range.isOneFullyContained)
    |> List.length

let countOverlappingPairs pairs =
    pairs
    |> List.filter (fun pair -> pair ||> Range.isOverlapping)
    |> List.length

let input = readInputLines "04" |> Seq.map Range.parsePair |> Seq.toList

let job1 () =
    input |> countFullyContainedPairs |> string

let job2 () =
    input |> countOverlappingPairs |> string
