module Day05

open Utils

type Point = { X: int; Y: int }

module Point =
    let ofString string =
        let numbers = string |> String.split ',' |> Array.map int

        { X = numbers.[0]; Y = numbers.[1] }

type Line = { P1: Point; P2: Point }

module Line =
    let ofString (string: string) =
        let points = string.Split(" -> ") |> Array.map Point.ofString

        { P1 = points.[0]; P2 = points.[1] }

    open System

    let points { P1 = p1; P2 = p2 } =
        let dx = Math.Sign(p2.X - p1.X)
        let dy = Math.Sign(p2.Y - p1.Y)

        let n = Math.Max(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y)) + 1

        seq {
            let mutable p = p1

            for _ = 1 to n do
                yield p
                p <- { X = p.X + dx; Y = p.Y + dy }
        }

type LineFilter =
    | WithDiagonals
    | WithoutDiagonals

let countMostDangerousPoints filter lines =
    let incrementPoint map point =
        map
        |> Map.change point (function
            | None -> 1 |> Some
            | Some n -> n + 1 |> Some)

    let addPoints map line =
        line |> Line.points |> Seq.fold incrementPoint map

    let isDiagonal line =
        line.P1.X <> line.P2.X && line.P1.Y <> line.P2.Y

    lines
    |> List.map Line.ofString
    |> List.filter (fun line ->
        match filter with
        | WithDiagonals -> true
        | WithoutDiagonals -> line |> isDiagonal |> not)
    |> List.fold addPoints Map.empty
    |> Map.values
    |> Seq.filter (fun value -> value >= 2)
    |> Seq.length

let input = "05" |> readInputLines |> Seq.toList

let job1 () =
    input |> countMostDangerousPoints WithoutDiagonals |> string

let job2 () =
    input |> countMostDangerousPoints WithDiagonals |> string
