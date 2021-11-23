module Day03

open System.Text.RegularExpressions
open Utils

type Object =
    | OpenSpace
    | Tree

type Slope = int * int

let walk ((dx, dy): Slope) (map: Object array array) =
    let height = map |> Array.length
    let width = map.[0] |> Array.length
    seq {
        let mutable x = 0
        for y in 0 .. dy .. (height - 1) do
            let x' = x % width
            let object = map.[y].[x']
            yield object
            x <- x + dx
    }

let toObject =
    function
    | '.' -> OpenSpace
    | '#' -> Tree
    | c -> failwithf "Invalid object symbol '%c'" c

let parse line = line |> Seq.map toObject |> Seq.toArray

let parseLines lines = lines |> Seq.map parse |> Seq.toArray

let job1 () =
    readInputLines "03"
    |> parseLines
    |> walk (Slope(3, 1))
    |> Seq.filter (function
        | Tree -> true
        | _ -> false)
    |> Seq.length
    |> string

let job2 () = ""
