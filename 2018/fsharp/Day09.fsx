#load "Utils.fsx"
open Utils

type Node = { Value: int; mutable Next: Node | null; mutable Prev: Node | null }

let initialNode () =
    let node = { Value = 0; Next = null; Prev = null }
    node.Next <- node
    node.Prev <- node
    node

let append value node =
    let newNode = { Value = value; Next = node.Next; Prev = node }
    node.Next.Prev <- newNode
    node.Next <- newNode
    newNode

let remove node =
    node.Prev.Next <- node.Next
    node.Next.Prev <- node.Prev
    node.Next

let play i node =
    let rec prev n node = if n = 0 then node else prev (n - 1) node.Prev

    match i % 23 with
    | 0 ->
        let marble = node |> prev 7
        let score = i + marble.Value
        let node' = remove marble
        score, node'
    | _ -> 0, append i node.Next

let playGame playerCount marbleCount =
    let scores = Array.zeroCreate<int64> playerCount

    let rec loop node i =
        if i < marbleCount then
            let score, node' = play (i + 1) node
            let index = i % playerCount
            scores.[index] <- scores.[index] + int64 score
            loop node' (i + 1)

    loop (initialNode ()) 0
    Array.max scores

let parseInput =
    let rx = @"(\d+) players; last marble is worth (\d+) points"

    function
    | Regex rx [ Int playerCount; Int marbleCount ] -> playerCount, marbleCount
    | line -> failwithf "Invalid input: %A" line

let part1 input = input |> parseInput ||> playGame

let part2 input =
    input
    |> parseInput
    |> fun (playerCount, marbleCount) -> playerCount, marbleCount * 100
    ||> playGame

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Part 1" 405143L solution1
Test.run "Part 2" 3411514667L solution2

#load "_benchmark.fsx"
