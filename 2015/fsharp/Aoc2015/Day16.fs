module Day16

open Utils

let data =
    [ ("children", 3)
      ("cats", 7)
      ("samoyeds", 2)
      ("pomeranians", 3)
      ("akitas", 0)
      ("vizslas", 0)
      ("goldfish", 5)
      ("trees", 3)
      ("cars", 2)
      ("perfumes", 1) ]
    |> Map.ofList

let parseSue =
    function
    | Regex @"^Sue (\d+): (.+)$" [ sue; compounds ] ->
        sue,
        compounds.Split(", ")
        |> Array.fold
            (fun map compound ->
                match compound with
                | Regex @"^(\w+): (\d+)$" [ compound; count ] ->
                    map |> Map.add compound (int count)
                | _ -> failwith $"Invalid compound: %s{compound}")
            Map.empty
    | sue -> failwith $"Invalid sue: %s{sue}"

let isMatching (_, compounds) =
    compounds |> Map.forall (fun compound count -> count = data.[compound])

let findSue sues = sues |> List.find isMatching |> fst

let isReallyMatching (_, compounds) =
    compounds
    |> Map.forall (fun compound count ->
        (count, data.[compound])
        ||> match compound with
            | "cats"
            | "trees" -> (>)
            | "pomeranians"
            | "goldfish" -> (<)
            | _ -> (=))

let findRealSue sues =
    sues |> List.find isReallyMatching |> fst

let input = readInputLines "16" |> List.ofSeq |> List.map parseSue

let job1 () = input |> findSue

let job2 () = input |> findRealSue
