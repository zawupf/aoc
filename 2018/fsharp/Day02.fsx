#load "Utils.fsx"
open Utils

let part1 input =
    input
    |> Array.map (fun line ->
        let counts =
            line
            |> String.toCharArray
            |> Array.countBy id
            |> Array.map snd
            |> Array.distinct

        (if Array.contains 2 counts then 1 else 0),
        (if Array.contains 3 counts then 1 else 0))
    |> Array.unzip
    |> fun (twos, threes) -> Array.sum twos * Array.sum threes

let part2 input =
    let diffs a b =
        Seq.zip a b
        |> Seq.indexed
        |> Seq.choose (fun (i, (x, y)) -> if x <> y then Some i else None)
        |> Seq.toList

    let rec tryFindSingleDiff (x: string) xs =
        match xs with
        | [] -> None
        | y :: ys ->
            match diffs x y with
            | [ i ] -> Some(x.Remove(i, 1))
            | _ -> tryFindSingleDiff x ys

    let rec findSingleDiff xs =
        match xs with
        | [] -> failwith "No solution"
        | x :: xs ->
            match tryFindSingleDiff x xs with
            | Some s -> s
            | None -> findSingleDiff xs

    input |> Array.toList |> findSingleDiff

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 4693 solution1
Test.run "Part 2" "pebjqsalrdnckzfihvtxysomg" solution2

#load "_benchmark.fsx"
