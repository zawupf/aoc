module Day03

open Utils

type House = int * int

let nextHouse direction (x, y) =
    match direction with
    | '^' -> x, y + 1
    | 'v' -> x, y - 1
    | '>' -> x + 1, y
    | '<' -> x - 1, y
    | _ -> failwithf "Invalid direction %c" direction

let walk houses direction =
    let nextHouse = houses |> List.head |> nextHouse direction
    nextHouse :: houses

let visitHouses directions = directions |> Array.fold walk [ 0, 0 ]

let visitedHousesCount input =
    input |> String.toCharArray |> visitHouses |> List.distinct |> List.length

let visitedHousesWithRoboCount input =
    let santaDirections, roboDirections =
        input
        |> String.toCharArray
        |> Array.chunkBySize 2
        |> Array.map (fun a -> a.[0], a.[1])
        |> Array.unzip

    santaDirections
    |> visitHouses
    |> List.append (roboDirections |> visitHouses)
    |> List.distinct
    |> List.length

let input = readInputText "03"

let job1 () = input |> visitedHousesCount |> string

let job2 () =
    input |> visitedHousesWithRoboCount |> string
