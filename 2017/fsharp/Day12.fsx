#load "Utils.fsx"
open Utils

let parse lines =
    lines
    |> Array.collect (fun line ->
        match line with
        | Regex @"(\d+) <-> (\d+(?:, \d+)*)$" [ Int id; ids ] ->
            ids
            |> String.parseInts ", "
            |> Array.collect (fun id2 -> [| id, id2; id2, id |])
        | _ -> failwithf "Invalid input: %s" line)
    |> Array.fold
        (fun map (id1, id2) ->
            map
            |> Dictionary.update id1 (function
                | Some set -> set |> Set.add id2
                | None -> Set.singleton id2))
        (Dictionary())

let group id map =
    let rec loop ids group =
        match ids with
        | [] -> group
        | id :: ids ->
            let group = group |> Set.add id

            let ids' =
                Set.difference
                    (map
                     |> Dictionary.tryGetValue id
                     |> Option.defaultValue Set.empty)
                    group
                |> Set.toList

            loop (ids' @ ids) group

    loop [ id ] Set.empty

let countGroups map =
    let rec loop ids count =
        if Set.isEmpty ids then
            count
        else
            let id = ids |> Seq.head
            let group = group id map
            let ids = Set.difference ids group
            loop ids (count + 1)

    loop (map |> Dictionary.keys |> Set.ofSeq) 0

let part1 input = input |> parse |> group 0 |> Set.count

let part2 input = input |> parse |> countGroups

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 6 (fun () -> part1 testInput[0])
Test.run "Test 2" 2 (fun () -> part2 testInput[0])

Test.run "Part 1" 141 solution1
Test.run "Part 2" 171 solution2

#load "_benchmark.fsx"
