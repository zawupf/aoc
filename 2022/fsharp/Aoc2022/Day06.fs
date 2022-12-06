module Day06

open Utils

let countCharsForStartOfMarker length signal =
    signal
    |> Seq.windowed length
    |> Seq.indexed
    |> Seq.find (snd >> Array.distinct >> Array.length >> (=) length)
    |> fst
    |> (+) length

let countCharsForStartOfPacketMarker = countCharsForStartOfMarker 4

let countCharsForStartOfMessageMarker = countCharsForStartOfMarker 14

let input = readInputText "06"

let job1 () =
    input |> countCharsForStartOfPacketMarker |> string

let job2 () =
    input |> countCharsForStartOfMessageMarker |> string
