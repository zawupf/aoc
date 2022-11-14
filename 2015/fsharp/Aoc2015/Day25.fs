module Day25

open Utils

let codes () =
    Seq.unfold
        (fun (row, col, code) ->
            let row', col' = if row = 1 then col + 1, 1 else row - 1, col + 1
            let code' = code * 252533UL % 33554393UL
            Some((row, col, code), (row', col', code')))
        (1, 1, 20151125UL)

let codeAt row col =
    codes ()
    |> Seq.pick (fun (r, c, code) ->
        if r = row && c = col then Some code else None)

let parse input =
    match input with
    | Regex @"row (\d+), column (\d+)" [ row; col ] -> int row, int col
    | _ -> failwith "Invalid input"

let input = readInputText "25" |> parse

let job1 () = input ||> codeAt |> string

let job2 () = "Ho Ho Ho!"
