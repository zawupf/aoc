module Day10

open System
open Utils

let parse lines =
    lines
    |> Seq.indexed
    |> Seq.collect (fun (y, line) ->
        line
        |> Seq.indexed
        |> Seq.choose (function
            | x, '#' -> Some(x, y)
            | _, '.' -> None
            | _, c -> failwithf "Invalid char: '%c'" c))
    |> Seq.toList

let private equal (x1, y1) (x2, y2) = x1 = x2 && y1 = y2

let private transition (x1, y1) (x2, y2) = x2 - x1, y2 - y1

let private distance a b =
    let (x, y) = transition a b
    (abs x) + (abs y)

let private direction a b =
    let (x, y) = transition a b
    let gcd = GCD x y
    x / gcd, y / gcd

let private numVisible asteroids pos =
    asteroids
    |> List.filter (not << equal pos)
    |> List.distinctBy (direction pos)
    |> List.length

let maxVisible asteroids =
    asteroids
    |> List.map (fun pos -> (numVisible asteroids pos, pos))
    |> List.maxBy fst

let private appendRotationAndAngle laser ((x, y), asteroids) =
    let angle =
        match Math.Atan2(float x, float -y) with
        | a when a < 0. -> a + 2. * Math.PI
        | a -> a
    asteroids
    |> List.sortBy (distance laser)
    |> List.mapi (fun index asteroid -> asteroid, (index, angle))

let private compareRotationThenAngle (_, (i1, a1)) (_, (i2, a2)) =
    match (compare i1 i2) with
    | 0 -> compare a1 a2
    | c -> c

let vaporizeOrder asteroids =
    let laser =
        asteroids
        |> maxVisible
        |> snd
    asteroids
    |> List.filter (not << equal laser)
    |> List.groupBy (direction laser)
    |> List.collect (appendRotationAndAngle laser)
    |> List.sortWith compareRotationThenAngle
    |> List.map fst

let job1() =
    readInputLines "10"
    |> parse
    |> maxVisible
    |> fst
    |> string

let job2() =
    readInputLines "10"
    |> parse
    |> vaporizeOrder
    |> List.item 199
    |> (fun (x, y) -> 100 * x + y)
    |> string
