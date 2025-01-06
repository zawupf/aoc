#load "Utils.fsx"

type Direction =
    | Left
    | Right

module Direction =
    let parse =
        function
        | 'L' -> Left
        | 'R' -> Right
        | d -> failwithf "Invalid direction: %A" d

type Node = { Name: string; Left: string; Right: string }

module Node =
    open Utils.FancyPatterns

    let parse =
        function
        | Regex (@"^(\w{3}) = \((\w{3}), (\w{3})\)$") [ name; left; right ] -> {
            Name = name
            Left = left
            Right = right
          }
        | line -> failwithf "Invalid node: %A" line

let parse input =
    let directions =
        seq {
            let directions =
                input
                |> Array.item 0
                |> Utils.String.toCharArray
                |> Array.map Direction.parse
                |> Array.toSeq

            while true do
                yield! directions
        }

    let nodes =
        input
        |> Array.skip 2
        |> Array.map (fun line ->
            let n = Node.parse line
            n.Name, n)
        |> Map.ofArray

    directions, nodes

let walk directions nodesMap predicate startNode =
    seq {
        let mutable node = startNode

        for d in directions do
            yield node

            let next =
                match d with
                | Left -> node.Left
                | Right -> node.Right

            node <- Map.find next nodesMap

    }
    |> Seq.takeWhile predicate
    |> Seq.length
    |> int64

let part1 input = //
    let directions, nodesMap = parse input

    walk
        directions
        nodesMap
        (_.Name.Equals("ZZZ") >> not)
        (Map.find "AAA" nodesMap)

let part2 input = //
    let directions, nodesMap = parse input

    nodesMap
    |> Map.values
    |> Seq.filter _.Name.EndsWith("A")
    |> Seq.map (walk directions nodesMap (_.Name.EndsWith("Z") >> not))
    |> Seq.fold Utils.Math.leastCommonMultiple 1L



let testInput =
    [|
        """
RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
"""
        """
LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
"""
        """
LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)
"""
    |]
    |> (Array.map Utils.String.toLines)

Utils.Test.run "Test part 1" 2L (fun () -> part1 testInput[0])
Utils.Test.run "Test part 1" 6L (fun () -> part1 testInput[1])
Utils.Test.run "Test part 2" 6L (fun () -> part2 testInput[2])



let input = Utils.readInputLines "08"

let getDay08_1 () = part1 input

let getDay08_2 () = part2 input

Utils.Test.run "Part 1" 20093L getDay08_1
Utils.Test.run "Part 2" 22103062509257L getDay08_2
