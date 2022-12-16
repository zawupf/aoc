module Day16

open Utils

let input = readInputLines "16"

type Valve =
    { Name: string
      Rate: int
      Open: bool
      Valves: int array }

type Runner = { Current: int; RemainingTime: int }

type Waypoint =
    { Runners: Runner list
      Pressure: int
      Targets: int list }

let parseLine line =
    let rx =
        @"^Valve (\w{2}) has flow rate=(\d+); tunnels? leads? to valves? (\w{2}(?:, \w{2})*)$"

    match line with
    | Regex rx [ name; Int rate; valves ] ->
        { Name = name
          Rate = rate
          Open = false
          Valves = Array.empty },
        valves.Split(", ")
    | _ -> failwith $"Invalid line: %s{line}"

let mapValveIndices (valves: _[], tunnels) =
    tunnels
    |> Array.mapi (fun i tunnels ->
        let valve = valves[i]

        let valves' =
            tunnels
            |> Array.map (fun name ->
                valves |> Array.findIndex (fun valve -> valve.Name = name))

        { valve with Valves = valves' })

let parse lines =
    lines |> Array.map parseLine |> Array.unzip |> mapValveIndices

let buildGraph valves =
    valves
    |> Seq.indexed
    |> Seq.fold
        (fun graph (i, { Valves = valves }) ->
            valves |> Array.fold (fun graph j -> ((i, j), 1) :: graph) graph)
        List.empty
    |> Map.ofSeq

let shortestPathLength graph i j =
    let rec loop result pathes =
        match pathes with
        | [] -> result
        | (i, len, visited) :: pathes ->
            match i = j with
            | true ->
                loop
                    (result |> Option.map (min len) |> Option.orElse (Some len))
                    pathes
            | false ->
                let pathes' =
                    graph
                    |> Map.fold
                        (fun pathes (i', j') l ->
                            let len' = len + l

                            if
                                i = i'
                                && visited |> Set.contains j' |> not
                                && result
                                   |> Option.map (fun r -> len' < r)
                                   |> Option.defaultValue true
                            then
                                (j', len', visited |> Set.add j') :: pathes
                            else
                                pathes)
                        pathes

                loop result pathes'

    loop None [ i, 0, Set.ofList [ i ] ]

let buildDistanceMap (valves: _[]) =
    let graph = buildGraph valves

    let rec loop result valves =
        match valves with
        | [] -> result |> Map.ofList
        | i :: js ->
            let paths = shortestPathLength graph i

            let len j =
                match paths j with
                | Some len -> len
                | None -> failwith $"Invalid path: %d{i} -> %d{j}"

            let result' =
                js
                |> List.fold
                    (fun result j ->
                        let l = len j
                        ((i, j), l) :: ((j, i), l) :: result)
                    result

            loop result' js

    loop [] [ 0 .. valves.Length - 1 ]

let maxPressure numRunners totalTime lines =
    let valves = lines |> parse
    let distances = valves |> buildDistanceMap

    let start = valves |> Array.findIndex (fun valve -> valve.Name = "AA")

    let targets =
        [ 0 .. valves.Length - 1 ] |> List.filter (fun i -> valves[i].Rate <> 0)

    let rec loop result ways =
        match ways with
        | [] -> result
        | waypoint :: ways ->
            let { Runners = allRunners
                  Pressure = p
                  Targets = js } =
                waypoint

            let runners =
                allRunners
                |> List.maxBy (fun r -> r.RemainingTime)
                |> List.singleton

            let result', ways' =
                js
                |> List.fold
                    (fun (r, ways) j ->
                        runners
                        |> List.fold
                            (fun (r, ways) runner ->
                                let { Current = i; RemainingTime = t } =
                                    runner

                                let others =
                                    allRunners
                                    |> List.removeAt (
                                        allRunners
                                        |> List.findIndex (fun r ->
                                            r = runner)
                                    )

                                let t' = t - Map.find (i, j) distances - 1

                                if t' <= 0 then
                                    if others |> List.isEmpty then
                                        r, ways
                                    else
                                        r,
                                        { waypoint with Runners = others }
                                        :: ways
                                else
                                    let p' = p + t' * valves[j].Rate
                                    let r' = max p' r
                                    let js' = js |> List.except [ j ]

                                    r',
                                    { Runners =
                                        { Current = j; RemainingTime = t' }
                                        :: others
                                      Pressure = p'
                                      Targets = js' }
                                    :: ways)
                            (r, ways))
                    (result, ways)

            loop result' ways'

    loop
        0
        [ { Runners =
              List.replicate
                  numRunners
                  { Current = start
                    RemainingTime = totalTime }
            Pressure = 0
            Targets = targets } ]

let job1 () = input |> maxPressure 1 30 |> string

let job2 () = input |> maxPressure 2 26 |> string
