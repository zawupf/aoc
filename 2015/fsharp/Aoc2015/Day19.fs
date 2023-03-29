module Day19

open Utils

let parse =
    function
    | Regex @"^(\w+) => (\w+)$" [ src; dst ] -> (src, dst)
    | input -> failwith $"Invalid input: %s{input}"

let parseInput input =
    let lines =
        input
        |> Seq.filter (fun line -> line |> String.length <> 0)
        |> Seq.toList

    let replacements = lines |> List.take (lines.Length - 1) |> List.map parse
    let molecule = lines |> List.last
    replacements, molecule

let rec mutate (src: string) dst (input: string) (i: int) results =
    match input.IndexOf(src, i) with
    | -1 -> results
    | index ->
        let output =
            input.Substring(0, index)
            + dst
            + input.Substring(index + src.Length)

        mutate src dst input (index + src.Length) (output :: results)

let generateMoleculesCount input replacements =
    replacements
    |> List.fold (fun results (src, dst) -> mutate src dst input 0 results) []
    |> List.distinct
    |> List.length

let reduceMoleculeMinCount input replacements =
    let next input replacements =
        replacements
        |> List.fold (fun results (src, dst) -> mutate dst src input 0 results) []
        |> List.distinct

    let rec loop replacements stack count =
        match stack with
        | [] -> count
        | (n, "e") :: _tail ->
            // Quick and dirty: Return first result and skip searching for a shorter one
            // loop replacements tail (min n count)
            n
        | (n, input) :: tail ->
            let stack' =
                if n < count - 1 then
                    next input replacements
                    |> List.fold
                        (fun stack molecule -> (n + 1, molecule) :: stack)
                        tail
                else
                    tail

            loop replacements stack' count

    let replacements' =
        replacements |> List.sortByDescending (snd >> String.length)

    loop replacements' [ 0, input ] System.Int32.MaxValue

let replacements, molecule = readInputLines "19" |> parseInput

let job1 () =
    replacements |> generateMoleculesCount molecule |> string

let job2 () =
    replacements |> reduceMoleculeMinCount molecule |> string
