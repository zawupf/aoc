#load "Utils.fsx"

type Towels = {
    Patterns: string[]
    Designs: string[]
}

let parse (lines: string[]) =
    let patterns = lines[0] |> Utils.String.split ", "
    let designs = lines[2..]

    {
        Patterns = patterns
        Designs = designs
    }

let countPossibleDesigns towels =
    let {
            Patterns = patterns
            Designs = designs
        } =
        towels

    let rec isPossibleDesign (design: string) =
        match design with
        | "" -> true
        | _ ->
            patterns
            |> Seq.filter design.StartsWith
            |> Seq.exists (fun pattern ->
                design.[pattern.Length ..] |> isPossibleDesign)

    designs |> Seq.filter isPossibleDesign |> Seq.length

let countAllPatternCombinations towels =
    let {
            Patterns = patterns
            Designs = designs
        } =
        towels

    let cache = System.Collections.Generic.Dictionary<string, int64>()

    let rec countCombinations (design: string) =
        match design with
        | "" -> 1L
        | _ ->
            match cache.TryGetValue design with
            | true, result -> result
            | false, _ ->
                let result =
                    patterns
                    |> Seq.filter design.StartsWith
                    |> Seq.sumBy (fun pattern ->
                        design[pattern.Length ..] |> countCombinations)

                cache.Add(design, result)
                result

    designs |> Array.sumBy countCombinations

let part1 input = input |> parse |> countPossibleDesigns

let part2 input =
    input |> parse |> countAllPatternCombinations

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 333 solution1
Utils.Test.run "Part 2" 678536865274732L solution2
