#load "Utils.fsx"
open System.Collections.Generic

type Node = Node of string

type Conn = Conn of Node * Node

module Conn =
    let create node1 node2 = Conn(min node1 node2, max node1 node2)

let buildData lines =
    lines
    |> Array.fold
        (fun (connections, graph) line ->
            let node, nodes' =
                match line |> Utils.String.split ':' with
                | [| name; names |] ->
                    Node name,
                    names |> Utils.String.splitNoEmpty ' ' |> Array.map Node
                | _ -> failwithf "Invalid input: %A" line

            let append node =
                function
                | Some set -> set |> Set.add node |> Some
                | None -> Set.empty |> Set.add node |> Some

            nodes'
            |> Array.fold
                (fun (connections, graph) node' ->
                    let connections =
                        connections |> Set.add (Conn.create node node')

                    let graph =
                        graph
                        |> Map.change node (append node')
                        |> Map.change node' (append node)

                    connections, graph)
                (connections, graph))
        (Set.empty, Map.empty)
    |> (fun (connections, graph) -> connections |> Set.toArray, graph)

type Path = { Length: int; Visited: Set<Node>; Connections: Conn list }

let walk graph start end' =
    let rec loop result =
        function
        | [] -> result
        | (_, path) :: paths when path.Length >= result.Length ->
            loop result paths
        | (node, path) :: paths when node = end' -> loop path paths
        | (node, path) :: paths ->
            let next = Set.difference (graph |> Map.find node) path.Visited

            let paths =
                next
                |> Seq.fold
                    (fun paths next ->
                        let path = {
                            Length = path.Length + 1
                            Visited = path.Visited.Add(next)
                            Connections =
                                Conn.create node next :: path.Connections
                        }

                        (next, path) :: paths)
                    paths

            loop result paths

    loop
        {
            Length = System.Int32.MaxValue
            Visited = Set.empty
            Connections = []
        }
        [
            start,
            {
                Length = 0
                Visited = Set.empty |> Set.add start
                Connections = []
            }
        ]
    |> _.Connections


let part1 input =
    let connections, graph = input |> buildData
    (connections.Length, graph.Count) |> Utils.dump |> ignore

    let nodes = List.ofSeq graph.Keys

    let densityMap = Dictionary<Conn, int>()

    nodes.Tail
    |> List.iter (fun node ->
        let connections = walk graph nodes.Head node

        connections
        |> List.iter (fun conn ->
            densityMap[conn] <- densityMap.GetValueOrDefault(conn) + 1))

    let densityMap =
        densityMap
        |> Seq.groupBy _.Value
        |> Seq.sortByDescending fst
        |> Seq.take 3
        |> Utils.dump

    0

let part2 input = 0



let testInput = [|
    """
jqt: rhn xhk nvd
rsh: frs pzl lsr
xhk: hfx
cmg: qnr nvd lhk bvb
rhn: xhk bvb hfx
bvb: xhk hfx
pzl: lsr hfx nvd
qnr: nvd
ntq: jqt hfx bvb xhk
nvd: lhk
lsr: lhk
rzs: qnr cmg lsr rsh
frs: qnr lhk lsr
"""
    |> Utils.String.toLines
|]

// Utils.Test.run "Test part 1" 54 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 1" 0 (fun () -> part1 testInput[0])
// Utils.Test.run "Test part 2" 0 (fun () -> part2 testInput[0])



let input = Utils.readInputLines "25"

let getDay25_1 () = part1 input

let getDay25_2 () = part2 input

Utils.Test.run "Part 1" 0 getDay25_1
// Utils.Test.run "Part 2" 0 getDay25_2
