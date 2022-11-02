module Day06

open System.Text.RegularExpressions

open Utils

type Pos = int * int

type Range = Pos * Pos

type Action =
    | TurnOn of Range
    | TurnOff of Range
    | Toggle of Range

let parseAction input =
    let rx =
        Regex(@"^(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)$")

    match
        rx.Match(input).Groups
        |> Seq.map (fun group -> group.Value)
        |> Seq.toList
    with
    | [ _; action; a; b; c; d ] ->
        let range = (int a, int b), (int c, int d)

        match action with
        | "turn on" -> TurnOn(range)
        | "turn off" -> TurnOff(range)
        | "toggle" -> Toggle(range)
        | _ -> failwithf "Invalid input: %s" input
    | _ -> failwithf "Invalid input: %s" input

let handleAction rules (grid: int[]) action =
    let index (x, y) = x + y * 1000
    let ((a, b), (c, d)), act = rules grid action
    let x, y = min a c, min b d
    let x', y' = max a c, max b d

    seq {
        for i in y..y' do
            for j in x..x' -> i, j
    }
    |> Seq.fold
        (fun (grid: int[]) pos ->
            let i = pos |> index
            grid.[i] <- act i
            grid)
        grid


let rules1 (grid: int[]) action =
    match action with
    | TurnOn(range) -> range, (fun _i -> 1)
    | TurnOff(range) -> range, (fun _i -> 0)
    | Toggle(range) -> range, (fun i -> 1 - grid.[i])

let handleAction1 = handleAction rules1

let rules2 (grid: int[]) action =
    match action with
    | TurnOn(range) -> range, (fun i -> grid.[i] + 1)
    | TurnOff(range) -> range, (fun i -> max 0 (grid.[i] - 1))
    | Toggle(range) -> range, (fun i -> grid.[i] + 2)

let handleAction2 = handleAction rules2

let newGrid () = Array.replicate 1_000_000 0

let input = readInputLines "06"

let job1 () =
    input
    |> Seq.map parseAction
    |> Seq.fold handleAction1 (newGrid ())
    |> Array.sum
    |> string

let job2 () =
    input
    |> Seq.map parseAction
    |> Seq.fold handleAction2 (newGrid ())
    |> Array.sum
    |> string
