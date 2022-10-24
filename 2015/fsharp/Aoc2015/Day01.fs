module Day01

open Utils

let private floorFolder floor =
    function
    | '(' -> floor + 1
    | ')' -> floor - 1
    | _ -> floor

let floor (input: string) =
    input.ToCharArray() |> Array.fold floorFolder 0

let enterBasementPosition (input: string) =
    input.ToCharArray()
    |> Seq.ofArray
    |> Seq.scan floorFolder 0
    |> Seq.findIndex (fun floor -> floor = -1)

let input = readInputText "01"

let job1 () = input |> floor |> string

let job2 () =
    input |> enterBasementPosition |> string
