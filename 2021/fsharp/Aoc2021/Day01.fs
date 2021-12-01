module Day01

open Utils

let smartCompareCount offset (seq: int seq) =
    seq
    |> Seq.skip offset
    |> Seq.zip seq
    |> Seq.filter (fun (a, b) -> b > a)
    |> Seq.length

let countIncreasedSeaDepths = smartCompareCount 1

let countIncreasedSeaDepthWindows = smartCompareCount 3


let input =
    readInputLines "01" |> List.ofSeq |> List.map int

let job1 () =
    input |> countIncreasedSeaDepths |> string

let job2 () =
    input |> countIncreasedSeaDepthWindows |> string
