module Day01

open Utils

let smartCompareCount offset (list: int list) =
    list
    |> List.skip offset
    |> List.zip (list |> List.rev |> List.skip offset |> List.rev)
    |> List.filter (fun (a, b) -> b > a)
    |> List.length

let countIncreasedSeaDepths = smartCompareCount 1

let countIncreasedSeaDepthWindows = smartCompareCount 3


let input =
    readInputLines "01" |> List.ofSeq |> List.map int

let job1 () =
    input |> countIncreasedSeaDepths |> string

let job2 () =
    input |> countIncreasedSeaDepthWindows |> string
