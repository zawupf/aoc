module Day01

open Utils

let move dist (x, y) (dx, dy) = x + dist * dx, y + dist * dy

let move2 visited dist pos dir =
    let rec loop visited dist pos dir =
        match dist with
        | 0 -> pos, visited, false
        | _ ->
            let pos' = move 1 pos dir

            if visited |> Set.contains pos' then
                pos', visited, true
            else
                let visited' = visited |> Set.add pos'
                loop visited' (dist - 1) pos' dir

    loop visited dist pos dir

let rotate turn (dx, dy) =
    match turn with
    | "L" ->
        match dx, dy with
        | 0, 1 -> -1, 0
        | 0, -1 -> 1, 0
        | 1, 0 -> 0, 1
        | -1, 0 -> 0, -1
        | _ -> failwith "Invalid direction"
    | "R" ->
        match dx, dy with
        | 0, 1 -> 1, 0
        | 0, -1 -> -1, 0
        | 1, 0 -> 0, -1
        | -1, 0 -> 0, 1
        | _ -> failwith "Invalid direction"
    | _ -> failwith "Invalid turn"

let next step pos dir =
    match step with
    | Regex @"^([RL])(\d+)$" [ turn; dist ] ->
        let dir' = rotate turn dir
        let pos' = move (int dist) pos dir'
        pos', dir'
    | _ -> failwith "Invalid step"

let next2 visited step pos dir =
    match step with
    | Regex @"^([RL])(\d+)$" [ turn; dist ] ->
        let dir' = rotate turn dir
        let pos', visited', done' = move2 visited (int dist) pos dir'
        pos', dir', visited', done'
    | _ -> failwith "Invalid step"

let walk1 steps =
    let rec loop steps pos dir =
        match steps with
        | [] -> pos
        | step :: steps -> next step pos dir ||> loop steps

    loop steps (0, 0) (0, 1)

let walk2 steps =
    let rec loop visited steps pos dir =
        match steps with
        | [] -> pos
        | step :: steps ->
            let pos', dir', visited', done' = next2 visited step pos dir

            if done' then pos' else loop visited' steps pos' dir'

    loop (Set.ofList [ 0, 0 ]) steps (0, 0) (0, 1)

let distance (x, y) = abs x + abs y

let parse (input: string) = input.Split(", ") |> Array.toList

let input = (readInputText "01") |> parse

let job1 () = input |> walk1 |> distance |> string


let job2 () = input |> walk2 |> distance |> string
