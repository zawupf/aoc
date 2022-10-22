module Day13

open System
open Utils

module Game =
    type Tile =
        | Empty
        | Wall
        | Block
        | HorizontalPaddle
        | Ball

    module Tile =
        let ofInt =
            function
            | 0 -> Empty
            | 1 -> Wall
            | 2 -> Block
            | 3 -> HorizontalPaddle
            | 4 -> Ball
            | id -> failwithf "Invalid tile id: %d" id

    type Screen =
        { tiles: Map<int64 * int64, Tile>
          score: int64 }

    module Screen =
        let empty = { tiles = Map.empty; score = 0L }

        let count tile screen =
            screen.tiles |> Map.filter (fun _key value -> value = tile) |> Map.count

        let find tile screen =
            screen.tiles |> Map.tryFindKey (fun _key value -> value = tile)

    type Game =
        { computer: Computer.Context
          screen: Screen }

    let boot source =
        { computer = Computer.compile source
          screen = Screen.empty }

    let private handleOutput screen =
        function
        | [| -1L; 0L; score |] -> { screen with score = score }
        | [| x; y; id |] -> { screen with tiles = screen.tiles |> Map.add (x, y) (id |> int |> Tile.ofInt) }
        | _ -> failwith "Invalid output data"

    let private render game =
        let screen =
            game.computer.output.ToArray()
            |> Array.chunkBySize 3
            |> Array.fold handleOutput game.screen

        game.computer.output.Clear()
        { game with screen = screen }

    let rec start game =
        let event = game.computer |> Computer.runSilent

        match event with
        | Computer.Halted -> game |> render
        | Computer.Paused ->
            game
            |> render
            |> (fun game ->
                let paddlePos = game.screen |> Screen.find HorizontalPaddle
                let ballPos = game.screen |> Screen.find Ball

                let joystick =
                    match paddlePos, ballPos with
                    | Some (paddle, _), Some (ball, _) -> compare ball paddle
                    | _ -> 0

                game.computer.input.Enqueue(joystick |> int64)
                game)
            |> start
        | _ -> failwithf "Handle event: %A" event

    let count tile game = game.screen |> Screen.count tile

    let score game = game.screen.score

    let play4free game =
        game.computer.memory.[0] <- 2L
        game

let job1 () =
    readInputText "13" |> Game.boot |> Game.start |> Game.count Game.Block |> string

let job2 () =
    readInputText "13"
    |> Game.boot
    |> Game.play4free
    |> Game.start
    |> Game.score
    |> string
