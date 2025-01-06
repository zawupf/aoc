#load "Utils.fsx"

type Page = int
type Rule = Page * Page
type Update = Page[]

module Rule =
    let ofArray =
        function
        | [| a; b |] -> a, b
        | _ -> failwith "Invalid rule"

module Update =
    let isValid (rules: Rule[]) (update: Update) =
        rules
        |> Array.forall (fun (a, b) ->
            match
                update |> Array.tryFindIndex ((=) a),
                update |> Array.tryFindIndex ((=) b)
            with
            | Some i, Some j -> i < j
            | _ -> true)

    let fix (rules: Rule[]) (update: Update) =
        let rules =
            rules
            |> Array.filter (fun (a, b) ->
                update |> Array.contains a && update |> Array.contains b)

        let predecessorCounts = rules |> Array.countBy snd |> Map.ofArray

        update
        |> Array.map (fun page ->
            page,
            predecessorCounts |> Map.tryFind page |> Option.defaultValue 0)
        |> Array.sortBy snd
        |> Array.map fst

    let middlePage (update: Update) = update |> Array.item (update.Length >>> 1)

let parse input =
    match
        input |> Utils.String.toSections |> Array.map Utils.String.toLines
    with
    | [| ruleLines; updateLines |] ->
        ruleLines |> Array.map (Utils.String.parseInts '|' >> Rule.ofArray),
        updateLines |> Array.map (Utils.String.parseInts ',')
    | _ -> failwith "Invalid input"

let part1 input =
    let rules, updates = input |> parse
    let isValidUpdate = Update.isValid rules

    updates |> Array.filter isValidUpdate |> Array.sumBy Update.middlePage

let part2 input =
    let rules, updates = input |> parse
    let isValidUpdate = Update.isValid rules
    let fixUpdate = Update.fix rules

    updates
    |> Array.filter (not << isValidUpdate)
    |> Array.map fixUpdate
    |> Array.sumBy Update.middlePage

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    """
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
"""

Utils.Test.run "Test 1" 143 (fun () -> part1 testInput)
Utils.Test.run "Test 2" 123 (fun () -> part2 testInput)

Utils.Test.run "Part 1" 6612 solution1
Utils.Test.run "Part 2" 4944 solution2

#load "_benchmark.fsx"
