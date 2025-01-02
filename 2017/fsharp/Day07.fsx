#load "Utils.fsx"
open Utils

type Program = {
    Name: string
    Weight: int
    Disc: Set<string>
}

let parseProgram line =
    match line with
    | Regex @"^(\w+) \((\d+)\)(?: -> )?(.+)?$" [ name; Int weight; disc ] -> {
        Name = name
        Weight = weight
        Disc =
            (if System.String.IsNullOrEmpty disc then
                 [||]
             else
                 disc |> String.split ", ")
            |> Set.ofArray
      }
    | _ -> failwithf "Invalid input: %s" line

let parseMap lines =
    lines
    |> Array.map parseProgram
    |> Array.map (fun program -> program.Name, program)
    |> Map.ofArray

let findRoot map =
    let allNames = map |> Map.values |> Seq.map _.Disc |> Seq.reduce Set.union
    let allParents = map |> Map.keys |> Set.ofSeq
    Set.difference allParents allNames |> Seq.exactlyOne

let fixWeight map =
    let resolve name = map |> Map.find name
    let getTotalWeight = useCache<string, int> ()

    let rec totalWeight program =
        getTotalWeight program.Name
        <| fun _ ->
            let discWeight = program.Disc |> Seq.sumBy (resolve >> totalWeight)

            program.Weight + discWeight

    let rec findUnbalanced program expectedWeight =
        let faulty, ok =
            program.Disc
            |> Seq.map (fun name -> name, name |> resolve |> totalWeight)
            |> Seq.toArray
            |> Array.groupBy snd
            |> Array.partition (fun (_, progs) -> progs.Length = 1)

        if Array.isEmpty faulty then
            let discWeight = totalWeight program - program.Weight
            expectedWeight - discWeight
        else
            let expectedWeight = ok |> Array.exactlyOne |> fst

            let faulty =
                faulty |> Array.exactlyOne |> snd |> Array.exactlyOne |> fst

            findUnbalanced (faulty |> resolve) expectedWeight

    findUnbalanced (map |> findRoot |> resolve) 0

let part1 input = input |> parseMap |> findRoot

let part2 input = input |> parseMap |> fixWeight

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" "tknk" (fun () -> part1 testInput[0])
Test.run "Test 2" 60 (fun () -> part2 testInput[0])

Test.run "Part 1" "dtacyn" solution1
Test.run "Part 2" 521 solution2

#load "_benchmark.fsx"
