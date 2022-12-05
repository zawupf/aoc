module Day05

open Utils

type Crane =
    | Reverse
    | KeepOrder

let crane order n i j stacks =
    let movedCrates, remainingCrates = stacks |> Array.item i |> List.splitAt n

    let crates =
        match order with
        | Reverse -> movedCrates |> List.rev
        | KeepOrder -> movedCrates

    stacks[i] <- remainingCrates
    stacks[j] <- crates @ stacks[j]
    stacks

let craneOneByOne = crane Reverse

let craneAllAtOnce = crane KeepOrder

let move crane moves stacks =
    let rec loop stacks moves =
        match moves with
        | [] -> stacks
        | move :: moves ->
            match move with
            | Regex @"^move (\d+) from (\d+) to (\d+)$" [ Int n; Int i; Int j ] ->
                loop (crane n (i - 1) (j - 1) stacks) moves
            | _ -> failwith $"Invalid move: %s{move}"

    loop (stacks |> Array.copy) moves

let topCrates (stacks: char list array) =
    stacks |> Array.map List.head |> String.ofChars

let parse (input: string) =
    match input.Split("\n\n") with
    | [| stackInput; movesInput |] ->
        let stackInput = stackInput |> String.split '\n' |> Array.rev

        let indexes =
            stackInput
            |> Array.head
            |> Seq.indexed
            |> Seq.fold
                (fun indexes (i, c) ->
                    match c with
                    | ' ' -> indexes
                    | _ -> i :: indexes)
                []
            |> Seq.rev
            |> Seq.toArray

        let stacks = Array.replicate indexes.Length []

        stackInput
        |> Array.tail
        |> Array.iter (fun line ->
            indexes
            |> Array.iteri (fun iStack iChar ->
                if iChar < line.Length && line[iChar] <> ' ' then
                    stacks[iStack] <- line[iChar] :: stacks[iStack]))

        stacks, movesInput |> String.split '\n' |> Array.toList
    | _ -> failwith "Invalid input"

let input = readInputText "05"

let stacks, moves = input |> parse

let job1 () =
    stacks |> move craneOneByOne moves |> topCrates

let job2 () =
    stacks |> move craneAllAtOnce moves |> topCrates
