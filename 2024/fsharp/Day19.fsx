#load "Utils.fsx"
open System

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

type Span = {
    String: string
    Start: int
} with

    static member ofString string = { String = string; Start = 0 }

    member this.StartsWith start =
        this.String
            .AsSpan(this.Start)
            .StartsWith(start.String.AsSpan start.Start)

    member this.IsEmpty = this.Start >= this.String.Length
    member this.Length = this.String.Length - this.Start
    member this.Slice start = { this with Start = this.Start + start }

let countPossibleDesigns towels =
    let patterns = towels.Patterns |> Array.map Span.ofString
    let designs = towels.Designs |> Array.map Span.ofString

    let rec isPossibleDesign (design: Span) =
        match design.IsEmpty with
        | true -> true
        | _ ->
            patterns
            |> Seq.filter (fun pattern -> design.StartsWith pattern)
            |> Seq.exists (fun pattern ->
                isPossibleDesign (design.Slice pattern.Length))

    designs |> Array.filter isPossibleDesign |> Array.length

let countAllPatternCombinations towels =
    let patterns = towels.Patterns |> Array.map Span.ofString
    let designs = towels.Designs |> Array.map Span.ofString

    let cache = Collections.Generic.Dictionary<Span, int64>()

    let rec countCombinations (design: Span) =
        match design.IsEmpty with
        | true -> 1L
        | _ ->
            match cache.TryGetValue design with
            | true, result -> result
            | false, _ ->
                let result =
                    patterns
                    |> Seq.filter design.StartsWith
                    |> Seq.sumBy (fun pattern ->
                        design.Slice pattern.Length |> countCombinations)

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
