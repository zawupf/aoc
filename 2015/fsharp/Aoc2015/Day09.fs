module Day09

open Utils

let parseRoads input =
    let insert a b d list = (a, b, d) :: (b, a, d) :: list

    input
    |> Seq.fold
        (fun roads line ->
            match line with
            | Regex @"^(\w+) to (\w+) = (\d+)$" [ city1; city2; distance ] ->
                roads |> insert city1 city2 (distance |> int)
            | _ -> failwithf "Invalid input: %s" line)
        []

let cities roads =
    roads
    |> List.fold (fun cities (city, _, _) -> cities |> Set.add city) Set.empty
    |> Set.toList

let rec findRoutes (route, distance) roads =
    let current = route |> List.head
    let nextRoads = roads |> List.filter (fun (a, _, _) -> a = current)

    seq {
        match nextRoads with
        | [] -> yield route, distance
        | _ ->
            for (_, next, d) in nextRoads do
                yield!
                    findRoutes
                        (next :: route, distance + d)
                        (roads
                         |> List.filter (fun (a, b, _) ->
                             a <> current && b <> current))
    }


let routes roads =
    let cities = roads |> cities

    seq {
        for start in cities do
            yield!
                findRoutes ([ start ], 0) roads
                |> Seq.filter (fun (route, _) ->
                    route |> Seq.length = cities.Length)
    }

let input = readInputLines "09"

let job1 () =
    input |> parseRoads |> routes |> Seq.minBy snd |> snd |> string

let job2 () =
    input |> parseRoads |> routes |> Seq.maxBy snd |> snd |> string
