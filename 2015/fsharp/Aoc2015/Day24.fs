module Day24

open Utils

let distributePackages n packages =
    let sum = List.sum packages / n

    let rec loop results stack targetSum =
        match stack with
        | [] -> results
        | (sum, used, remaining) :: stack ->
            match remaining with
            | [] -> loop results stack targetSum
            | n :: ns ->
                match sum + n with
                | s when s = targetSum ->
                    let results' = (n :: used) :: results
                    loop results' stack targetSum
                | s when s < targetSum ->
                    let stack' = (s, n :: used, ns) :: (sum, used, ns) :: stack
                    loop results stack' targetSum
                | _ -> loop results stack targetSum

    loop [] [ 0, [], packages |> List.sort ] sum

let findMinimalConfigurations n packages =
    let configs = packages |> distributePackages n
    let minLength = configs |> List.map List.length |> List.min

    configs |> List.filter (fun list -> list |> List.length = minLength)

let quantumEntanglement group =
    group |> List.fold (fun product value -> product * int64 value) 1L

let findMinimalEntanglement n packages =
    packages
    |> findMinimalConfigurations n
    |> List.map quantumEntanglement
    |> List.min

let input = readInputLines "24" |> Seq.map int |> Seq.toList

let job1 () =
    input |> findMinimalEntanglement 3 |> string

let job2 () =
    input |> findMinimalEntanglement 4 |> string
