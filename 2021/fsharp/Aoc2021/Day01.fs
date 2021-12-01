module Day01

open Utils

let countIncreasedSeaDepths (seaDepths: int list) =
    seaDepths
    |> List.pairwise
    |> List.filter (fun (a, b) -> b > a)
    |> List.length

let countIncreasedSeaDepthWindows seaDepths =
    seaDepths
    |> List.windowed 3
    |> List.map List.sum
    |> countIncreasedSeaDepths


let input =
    readInputLines "01" |> List.ofSeq |> List.map int

let job1 () =
    input |> countIncreasedSeaDepths |> string

let job2 () =
    input |> countIncreasedSeaDepthWindows |> string
