module Day14

open System
open Utils

type Name =
    | ORE
    | FUEL
    | Name of string

module Name =
    let ofString =
        function
        | "ORE" -> ORE
        | "FUEL" -> FUEL
        | name -> Name name

type Chemical = Chemical of int64 * Name

module Chemical =
    let parse (string: string) =
        match string.Split(' ') with
        | [| count; name |] -> Chemical(count |> int64, name |> Name.ofString)
        | _ -> failwithf "Invalid chemical: %s" string

    let count (Chemical (count, _)) = count
    let name (Chemical (_, name)) = name

type Reaction = Reaction of Chemical list * Chemical

module Reaction =
    let private parseInput (string: string) =
        string.Split(',') |> Array.map (String.trim >> Chemical.parse) |> Array.toList

    let private parseOutput (string: string) =
        string |> (String.trim >> Chemical.parse)

    let parse (string: string) =
        match string.Split("=>") with
        | [| input; output |] -> Reaction(input |> parseInput, output |> parseOutput)
        | _ -> failwithf "Invalid reaction: %s" string

    let outputCount (Reaction (_, output)) = output |> Chemical.count
    let outputName (Reaction (_, output)) = output |> Chemical.name
    let inputs (Reaction (inputs, _)) = inputs

type Factory =
    { store: Map<Name, int64>
      reactions: Map<Name, Reaction> }

module Factory =
    let private parseReactions lines =
        lines
        |> Seq.map (fun line ->
            let reaction = line |> Reaction.parse
            reaction |> Reaction.outputName, reaction)
        |> Map.ofSeq

    let parse lines =
        { store = Map.empty
          reactions = lines |> parseReactions }

    let private available name factory =
        factory.store |> Map.tryFind name |> Option.defaultValue 0L

    let private add (Chemical (count, name)) factory =
        { factory with store = factory.store |> Map.add name ((factory |> available name) + count) }

    let private sub (Chemical (count, name)) = add (Chemical(-count, name))

    let private missing (Chemical (count, name)) factory =
        match name with
        | ORE -> 0L
        | _ -> count - (factory |> available name)
        |> (fun m -> if m < 0L then 0L else m)

    let rec private require (Chemical (_, name) as chemical) factory =
        let produce (Chemical (count, name)) factory =
            match count with
            | 0L -> factory
            | _ ->
                let reaction = factory.reactions |> Map.find name
                let outputCount = reaction |> Reaction.outputCount

                let num =
                    count / outputCount
                    + match count % outputCount with
                      | 0L -> 0L
                      | _ -> 1L

                reaction
                |> Reaction.inputs
                |> List.fold
                    (fun factory (Chemical (count, name)) ->
                        factory
                        |> require (Chemical(num * count, name))
                        |> sub (Chemical(num * count, name)))
                    factory
                |> add (Chemical(num * outputCount, name))

        factory |> produce (Chemical(factory |> missing chemical, name))

    let orePerFuel count factory =
        factory |> require (Chemical(count, FUEL)) |> available ORE |> abs

    let rec private fuelRange availableOre min factory =
        let max = 2L * min
        let ore = factory |> orePerFuel max

        if ore <= availableOre then
            factory |> fuelRange availableOre max
        else
            min, max

    let rec private searchFuelPerOre availableOre (min, max) factory =
        match (min + max) / 2L with
        | n when n = min -> min
        | n ->
            factory
            |> searchFuelPerOre
                availableOre
                (match factory |> orePerFuel n with
                 | ore when ore <= availableOre -> (n, max)
                 | _ -> (min, n))

    let fuelPerOre availableOre factory =
        let range = factory |> fuelRange availableOre 1L
        factory |> searchFuelPerOre availableOre range

let job1 () =
    readInputLines "14" |> Factory.parse |> Factory.orePerFuel 1L |> string

let job2 () =
    readInputLines "14"
    |> Factory.parse
    |> Factory.fuelPerOre 1000000000000L
    |> string
