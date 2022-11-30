module Day11

open System
open Utils
open Computer

module Robot =
    type Color =
        | Black
        | White

    module Color =
        let toLong =
            function
            | Black -> 0L
            | White -> 1L

        let ofLong =
            function
            | 0L -> Black
            | 1L -> White
            | color -> failwithf "Not a color: %d" color

    type Face =
        | Up
        | Down
        | Left
        | Right

    type Turn =
        | AntiClockwise
        | Clockwise

    module Turn =
        let toLong =
            function
            | AntiClockwise -> 0L
            | Clockwise -> 1L

        let ofLong =
            function
            | 0L -> AntiClockwise
            | 1L -> Clockwise
            | turn -> failwithf "Not a turn: %d" turn

    type State =
        { position: int * int
          face: Face
          grid: Map<int * int, Color>
          startColor: Color
          brain: Context }

    let paint state =
        let color = state.brain.output.Dequeue() |> Color.ofLong

        { state with
            grid = state.grid.Add(state.position, color) }

    let move state =
        let turn = state.brain.output.Dequeue() |> Turn.ofLong

        let face =
            match turn with
            | Clockwise ->
                match state.face with
                | Up -> Right
                | Right -> Down
                | Down -> Left
                | Left -> Up
            | AntiClockwise ->
                match state.face with
                | Up -> Left
                | Left -> Down
                | Down -> Right
                | Right -> Up

        let (x, y) = state.position

        let position =
            match face with
            | Up -> x, y - 1
            | Down -> x, y + 1
            | Left -> x - 1, y
            | Right -> x + 1, y

        { state with
            position = position
            face = face }

    let work = paint >> move

    let color state =
        state.grid
        |> Map.tryFind state.position
        |> Option.defaultWith (fun () ->
            if state.grid |> Map.isEmpty then
                state.startColor
            else
                Black)

    let boot startColor source =
        { position = (0, 0)
          face = Up
          grid = Map.empty
          startColor = startColor
          brain = compile source }

    let rec walk state =
        state.brain.input.Enqueue(state |> color |> Color.toLong)

        match runSilent state.brain with
        | Output _ -> failwith "Output event not possible in silent mode"
        | Halted -> state |> work
        | Paused -> state |> work |> walk

open Robot

let job1 () =
    readInputText "11"
    |> boot Black
    |> walk
    |> (fun state -> state.grid |> Map.count)
    |> string

let job2 () =
    readInputText "11"
    |> boot White
    |> walk
    |> (fun state ->
        state.grid
        |> render (function
            | Some White -> "#"
            | Some Black
            | None -> " "))
    |> string
