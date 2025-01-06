#load "Utils.fsx"

type Node = { X: int; Y: int }

module Node =
    let incX node = { node with X = node.X + 1 }
    let decX node = { node with X = node.X - 1 }
    let incY node = { node with Y = node.Y + 1 }
    let decY node = { node with Y = node.Y - 1 }

type Edge = { Start: Node; End: Node; Length: int }

module Edge =
    let swapped edge = { edge with Start = edge.End; End = edge.Start }

type Graph = { Start: Node; End: Node; Edges: Edge[] }

module Graph =
    open System.Collections.Generic

    let create lines =
        let height = Array.length lines
        let width = String.length lines[0]

        let mapGet node =
            if
                node.X >= 0 && node.X < width && node.Y >= 0 && node.Y < height
            then
                lines[node.Y][node.X] |> Some
            else
                None

        let visitedNodes = HashSet<Node>()
        let start = { X = 1; Y = 0 }
        visitedNodes.Add(start) |> ignore
        let end' = { X = width - 2; Y = height - 1 }
        visitedNodes.Add(end') |> ignore

        let rec loop edges (paths: (Edge * Node) list) =
            match paths with
            | [] -> { Start = start; End = end'; Edges = edges |> List.toArray }
            | (edge, _) :: paths' when edge.End = end' ->
                loop (edge :: edges) paths'
            | (edge, prev) :: paths' ->
                let mutable forkCount = 0

                let nextNodes =
                    [
                        Node.incX edge.End
                        Node.decX edge.End
                        Node.incY edge.End
                        Node.decY edge.End
                    ]
                    |> List.filter (fun node -> node <> prev)
                    |> List.choose (fun node ->
                        mapGet node
                        |> Option.bind (function
                            | '#' -> None
                            | '.' -> Some node
                            | c ->
                                forkCount <- forkCount + 1

                                if
                                    c = '<' && node.X < edge.End.X
                                    || c = '>' && node.X > edge.End.X
                                    || c = '^' && node.Y < edge.End.Y
                                    || c = 'v' && node.Y > edge.End.Y
                                then
                                    Some node
                                else
                                    None))

                let edges', paths' =
                    match nextNodes with
                    | [] -> failwithf "Strange next nodes: %A" nextNodes
                    | [ node ] when forkCount <= 1 ->
                        let path =
                            { edge with End = node; Length = edge.Length + 1 },
                            edge.End

                        edges, (path :: paths')
                    | _ ->
                        let edges' = edge :: edges

                        let paths =
                            match visitedNodes.Add(edge.End) with
                            | true ->
                                nextNodes
                                |> List.map (fun node ->
                                    let path =
                                        {
                                            Start = edge.End
                                            End = node
                                            Length = 1
                                        },
                                        edge.End

                                    path)
                            | false -> []

                        edges', (paths @ paths')

                loop edges' paths'

        loop [] [ { Start = start; End = start; Length = 0 }, start ]

    let buildMap graph =
        graph.Edges
        |> Array.fold
            (fun nodes edge -> nodes |> Set.add edge.Start |> Set.add edge.End)
            Set.empty
        |> Set.fold
            (fun map node ->
                let paths =
                    graph.Edges |> Array.filter (fun edge -> edge.Start = node)

                map |> Map.add node paths)
            Map.empty



type Path = { Node: Node; Visited: Set<Node>; Length: int }

let findMaxPath g =
    let m = Graph.buildMap g

    let rec loop result =
        function
        | [] -> result
        | { Node = node; Length = length } :: paths when node = g.End ->
            loop (max result length) paths
        | path :: paths ->
            loop
                result
                (m[path.Node]
                 |> Array.filter (fun edge ->
                     not <| path.Visited.Contains(edge.End))
                 |> Array.fold
                     (fun paths edge ->
                         let path = {
                             Node = edge.End
                             Visited = path.Visited.Add(edge.End)
                             Length = path.Length + edge.Length
                         }

                         path :: paths)
                     paths)

    loop 0 [ { Node = g.Start; Visited = Set.ofList [ g.Start ]; Length = 0 } ]

let part1 lines =
    let g = Graph.create lines
    findMaxPath g

let part2 lines =
    let g = Graph.create lines

    let g = {
        g with
            Edges = Array.append g.Edges (g.Edges |> Array.map Edge.swapped)
    }

    findMaxPath g



let testInput =
    """
#.#####################
#.......#########...###
#######.#########.#.###
###.....#.>.>.###.#.###
###v#####.#v#.###.#.###
###.>...#.#.#.....#...#
###v###.#.#.#########.#
###...#.#.#.......#...#
#####.#.#.#######.#.###
#.....#.#.#.......#...#
#.#####.#.#.#########v#
#.#...#...#...###...>.#
#.#.#v#######v###.###v#
#...#.>.#...>.>.#.###.#
#####v#.#.###v#.#.###.#
#.....#...#...#.#.#...#
#.#########.###.#.#.###
#...###...#...#...#.###
###.###.#.###v#####v###
#...#...#.#.>.>.#.>.###
#.###.###.#.###.#.#v###
#.....###...###...#...#
#####################.#
"""
    |> Utils.String.toLines

Utils.Test.run "Test part 1" 94 (fun () -> part1 testInput)
Utils.Test.run "Test part 2" 154 (fun () -> part2 testInput)


let input = Utils.readInputLines "23"

let getDay23_1 () = part1 input

let getDay23_2 () = part2 input

Utils.Test.run "Part 1" 2238 getDay23_1
Utils.Test.run "Part 2" 6398 getDay23_2
