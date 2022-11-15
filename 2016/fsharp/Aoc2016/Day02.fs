module Day02

open Utils

let moveInSquare moves key =
    moves
    |> Seq.fold
        (fun key direction ->
            match direction with
            | 'U' -> if key < 4 then key else (key - 3)
            | 'D' -> if key > 6 then key else (key + 3)
            | 'L' -> if (key - 1) % 3 = 0 then key else (key - 1)
            | 'R' -> if (key - 1) % 3 = 2 then key else (key + 1)
            | c -> failwithf $"Invalid direction '%c{c}'")
        key

let moveInDiamond moves key =
    moves
    |> Seq.fold
        (fun key direction ->
            match direction with
            | 'U' ->
                match key with
                | k when [| 1; 2; 4; 5; 9 |] |> Array.contains k -> k
                | k when [| 3; 13 |] |> Array.contains k -> k - 2
                | k -> k - 4
            | 'D' ->
                match key with
                | k when [| 5; 9; 10; 12; 13 |] |> Array.contains k -> k
                | k when [| 1; 11 |] |> Array.contains k -> k + 2
                | k -> k + 4
            | 'L' ->
                match key with
                | k when [| 1; 2; 5; 10; 13 |] |> Array.contains k -> k
                | k -> k - 1
            | 'R' ->
                match key with
                | k when [| 1; 4; 9; 12; 13 |] |> Array.contains k -> k
                | k -> k + 1
            | c -> failwithf $"Invalid direction '%c{c}'")
        key

let code move input =
    input
    |> List.scan (fun key moves -> move moves key) 5
    |> List.tail
    |> List.map (sprintf "%X")
    |> String.join ""

let input = readInputLines "02" |> Seq.toList

let job1 () = input |> code moveInSquare

let job2 () = input |> code moveInDiamond
