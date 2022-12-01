module Day01

open Utils

let parse lines =
    let rec loop result current lines =
        match lines with
        | [] ->
            match current with
            | [] -> result
            | _ -> current :: result
        | line :: lines ->
            match line with
            | Int n -> loop result (n :: current) lines
            | _ -> loop (current :: result) [] lines

    loop [] [] lines

let findMostCalories cals = cals |> List.map List.sum |> List.max

let findTop3Calories cals =
    cals |> List.map List.sum |> List.sortDescending |> List.take 3 |> List.sum

let input = readInputLines "01" |> Array.toList

let job1 () =
    input |> parse |> findMostCalories |> string

let job2 () =
    input |> parse |> findTop3Calories |> string
