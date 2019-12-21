module Day06

open System
open Utils

let private parseOrbit (text: string) =
    let names = text.Split(')')
    names.[0], names.[1]

let parse (lines: string seq) =
    lines
    |> Seq.mapFold (fun map line ->
        let (planet, moon) = parseOrbit line
        (), Map.add moon planet map) Map.empty
    |> snd

type OrbitMap = Map<string, string>

let planetOf moon (map: OrbitMap) = map.[moon]

let allPlanetsOf moon (map: OrbitMap) =
    let rec join planets =
        match planets with
        | "COM" :: _ -> planets
        | moon :: _ -> join <| (planetOf moon map) :: planets
        | [] -> []
    if moon = "COM" then [] else join [ planetOf moon map ]

let checksum orbits = orbits |> Map.fold (fun sum moon _planet -> sum + List.length (allPlanetsOf moon orbits)) 0

let minimalTransferCount orbits =
    let planets1 = allPlanetsOf "YOU" orbits
    let planets2 = allPlanetsOf "SAN" orbits
    (List.except planets1 planets2 |> List.length) + (List.except planets2 planets1 |> List.length)

let job1() =
    readInputLines "06"
    |> parse
    |> checksum
    |> string

let job2() =
    readInputLines "06"
    |> parse
    |> minimalTransferCount
    |> string
