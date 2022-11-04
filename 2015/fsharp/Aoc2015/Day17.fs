module Day17

open Utils

let combinations store sizes =
    let rec loop stack results =
        match stack with
        | [] -> results
        | (missing, combinations, sizes) :: remaining ->
            match missing with
            | 0 -> loop remaining (combinations @ results)
            | _ ->
                match sizes with
                | [] -> loop remaining results
                | n :: sizes when n <= missing ->
                    let combinations' =
                        match combinations with
                        | [] -> [ [ n ] ]
                        | _ -> combinations |> List.map (fun c -> n :: c)

                    loop
                        ((missing - n, combinations', sizes)
                         :: ((missing, combinations, sizes) :: remaining))
                        results
                | _ :: sizes ->
                    loop ((missing, combinations, sizes) :: remaining) results

    loop [ store, [], sizes ] []

let countCombinations store sizes = combinations store sizes |> List.length

let countMinimalCombinations store sizes =
    combinations store sizes
    |> List.countBy List.length
    |> List.minBy fst
    |> snd


let input = readInputLines "17" |> List.ofSeq |> List.map int

let job1 () =
    input |> countCombinations 150 |> string

let job2 () =
    input |> countMinimalCombinations 150 |> string
