#load "Utils.fsx"

[<RequireQualifiedAccess>]
type Cube =
    | Red
    | Green
    | Blue

module Cube =
    let parse string =
        match string with
        | "red" -> Cube.Red
        | "green" -> Cube.Green
        | "blue" -> Cube.Blue
        | _ -> failwithf "Invalid cube: %A" string

type CubeSet = CubeSet of Map<Cube, int>

module CubeSet =
    open Utils.FancyPatterns

    let isValidWithBag (CubeSet bag) (CubeSet set) =
        set
        |> Seq.forall (fun (KeyValue(cube, count)) ->
            bag |> Map.tryFind cube |> Option.defaultValue 0 >= count)

    let power (CubeSet set) =
        [ Cube.Red; Cube.Green; Cube.Blue ]
        |> List.map (fun cube ->
            set |> Map.tryFind cube |> Option.defaultValue 0)
        |> List.reduce (*)

    let parse string =
        string
        |> Utils.String.split ','
        |> Array.fold
            (fun map cube ->
                match cube with
                | Regex (@"(\d+) (red|green|blue)") [ Int n; color ] ->
                    map |> Map.add (Cube.parse color) n
                | _ -> failwithf "Invalid cube set: %A" string)
            Map.empty
        |> CubeSet

type Id =
    | Id of int

    member this.Value =
        let (Id value) = this
        value

type Game = { Id: Id; CubeSets: CubeSet list }

module Game =
    open Utils.FancyPatterns

    let isValidWithBag bag game =
        game.CubeSets |> Seq.forall (CubeSet.isValidWithBag bag)

    let findSmallestValidBag game =
        game.CubeSets
        |> List.reduce (fun bag (CubeSet set) ->
            set
            |> Seq.fold
                (fun (CubeSet bag) (KeyValue(cube, count)) ->
                    bag
                    |> Map.change cube (function
                        | Some n -> max n count |> Some
                        | None -> count |> Some)
                    |> CubeSet)
                bag)

    let parse string =
        let id, sets =
            match string with
            | Regex (@"Game (\d+): (.+)") [ Int id; sets ] ->
                Id id,
                sets
                |> Utils.String.split ';'
                |> Array.map CubeSet.parse
                |> Array.toList
            | _ -> failwithf "Invalid game: %A" string

        { Id = id; CubeSets = sets }

let part1 input =
    let bag =
        Map.ofList [ Cube.Red, 12; Cube.Green, 13; Cube.Blue, 14 ] |> CubeSet

    input
    |> Seq.map Game.parse
    |> Seq.filter (Game.isValidWithBag bag)
    |> Seq.sumBy _.Id.Value

let part2 input =
    input
    |> Seq.map (Game.parse >> Game.findSmallestValidBag)
    |> Seq.sumBy CubeSet.power



let testInput = [|
    """
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
"""
    |> Utils.String.toLines
|]

Utils.Test.run "Test part 1" 8 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 2286 (fun () -> part2 testInput[0])



let input = Utils.readInputLines "02"

let getDay02_1 () = part1 input

let getDay02_2 () = part2 input

Utils.Test.run "Part 1" 2204 getDay02_1
Utils.Test.run "Part 2" 71036 getDay02_2
