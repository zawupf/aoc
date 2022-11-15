module Day03

open Utils

type Triangle = int * int * int

module Triangle =
    let isMaybeTriangle (a, b, c) = a + b > c && a + c > b && b + c > a

    let parse string =
        string
        |> String.parseInts ' '
        |> function
            | [ a; b; c ] -> a, b, c
            | _ -> failwith $"Invalid triangle: %s{string}"

    let parseColums lines =
        lines
        |> List.map (String.parseInts ' ')
        |> List.transpose
        |> List.concat
        |> List.chunkBySize 3
        |> List.map (function
            | [ a; b; c ] -> a, b, c
            | _ -> failwith "Invalid columns")

let input = readInputLines "03" |> Seq.toList

let job1 () =
    input
    |> List.map Triangle.parse
    |> List.filter Triangle.isMaybeTriangle
    |> List.length
    |> string

let job2 () =
    input
    |> Triangle.parseColums
    |> List.filter Triangle.isMaybeTriangle
    |> List.length
    |> string
