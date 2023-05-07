module Day22

open Utils

type Tile =
    | Open
    | Wall
    | Offside

type Facing =
    | Right
    | Down
    | Left
    | Up

type Turn =
    | TurnRight
    | TurnLeft

let (|Turn|_|) (s: string) =
    match s with
    | "R" -> Some TurnRight
    | "L" -> Some TurnLeft
    | _ -> None

type Pos = { x: int; y: int }
type Cursor = { pos: Pos; facing: Facing }

type Scene = {
    map: Tile[,]
    cursor: Cursor
    path: string
}

module Tile =
    let toChar =
        function
        | Open -> '.'
        | Wall -> '#'
        | Offside -> ' '

module Facing =
    let toInt =
        function
        | Right -> 0
        | Down -> 1
        | Left -> 2
        | Up -> 3

    let turn direction cursor =
        let facing =
            match direction with
            | TurnRight ->
                match cursor.facing with
                | Right -> Down
                | Down -> Left
                | Left -> Up
                | Up -> Right
            | TurnLeft ->
                match cursor.facing with
                | Right -> Up
                | Up -> Left
                | Left -> Down
                | Down -> Right

        { cursor with facing = facing }

module Scene =
    let parse input =
        let stripEmptyLines lines =
            lines
            |> Seq.skipWhile (fun line ->
                System.String.IsNullOrWhiteSpace(line))
            |> Seq.rev
            |> Seq.skipWhile (fun line ->
                System.String.IsNullOrWhiteSpace(line))
            |> Seq.rev
            |> Seq.toArray

        let parseMap lines =
            let lines = Array.concat [ [| "" |]; lines; [| "" |] ]
            let height = lines |> Array.length
            let width = lines |> Seq.map String.length |> Seq.max |> (+) 2

            let lines =
                lines
                |> Array.map (fun (line: string) ->
                    let line = " " + line
                    line.PadRight(width))

            let map =
                Array2D.init width height (fun x y ->
                    match lines[y][x] with
                    | '.' -> Open
                    | '#' -> Wall
                    | ' ' -> Offside
                    | c -> failwithf "Invalid map character: %c" c)

            let cursor =
                [
                    for y in 1 .. height - 2 do
                        for x in 1 .. width - 2 do
                            if map[x, y] = Open then
                                yield {
                                    pos = { x = x; y = y }
                                    facing = Right
                                }
                ]
                |> List.head

            map, cursor

        let lines = input |> String.split '\n' |> stripEmptyLines
        let map, cursor = parseMap lines[.. lines.Length - 3]
        let path = lines |> Array.last |> String.trim

        {
            map = map
            cursor = cursor
            path = path
        }

    let dump scene =
        let width = Array2D.length1 scene.map
        let height = Array2D.length2 scene.map
        printfn "w: %d; h: %d" width height

        [
            for y in 1 .. height - 2 do
                for x in 1 .. width - 2 -> x, y, (Tile.toChar scene.map[x, y])
        ]
        |> List.iter (fun (x, _y, c) ->
            if x = 1 then
                printf "\n"

            printf "%c" c)

        printf "\n"

    let password scene =
        let { cursor = c } = scene
        1000 * c.pos.y + 4 * c.pos.x + Facing.toInt c.facing

    let private step cursor (map: _[,]) =
        let pos = cursor.pos

        let c =
            match cursor.facing with
            | Right -> {
                cursor with
                    pos = { pos with x = pos.x + 1 }
              }
            | Down -> {
                cursor with
                    pos = { pos with y = pos.y + 1 }
              }
            | Left -> {
                cursor with
                    pos = { pos with x = pos.x - 1 }
              }
            | Up -> {
                cursor with
                    pos = { pos with y = pos.y - 1 }
              }

        match map[c.pos.x, c.pos.y] with
        | Open -> c
        | Wall -> cursor
        | Offside ->
            let c =
                match cursor.facing with
                | Right ->
                    map[*, pos.y]
                    |> Array.findIndex (fun tile -> tile <> Offside)
                    |> (fun x -> { cursor with pos = { pos with x = x } })
                | Left ->
                    map[*, pos.y]
                    |> Array.findIndexBack (fun tile -> tile <> Offside)
                    |> (fun x -> { cursor with pos = { pos with x = x } })
                | Down ->
                    map[pos.x, *]
                    |> Array.findIndex (fun tile -> tile <> Offside)
                    |> (fun y -> { cursor with pos = { pos with y = y } })
                | Up ->
                    map[pos.x, *]
                    |> Array.findIndexBack (fun tile -> tile <> Offside)
                    |> (fun y -> { cursor with pos = { pos with y = y } })

            match map[c.pos.x, c.pos.y] with
            | Open -> c
            | Wall -> cursor
            | Offside -> failwith "Internal error"

    let private goForward steps cursor map =
        let rec loop steps cursor map =
            match steps with
            | 0 -> cursor
            | n when n > 0 ->
                match step cursor map with
                | next when next = cursor -> cursor
                | next -> loop (steps - 1) next map
            | _ -> failwith "Internal error"

        loop steps cursor map

    let walk scene =
        let rec loop path cursor map =
            match path with
            | "" -> {
                map = map
                cursor = cursor
                path = path
              }
            | Regex @"^(\d+)(.*?)$" [ Int steps; path ] ->
                let cursor = goForward steps cursor map
                loop path cursor map
            | Regex @"^([LR])(.*?)$" [ Turn direction; path ] ->
                let cursor = cursor |> Facing.turn direction
                loop path cursor map
            | _ -> failwithf "Invalid path: %s" path

        let {
                map = map
                cursor = cursor
                path = path
            } =
            scene

        loop path cursor map

let input = readInputExact "22"

let job1 () =
    input |> Scene.parse |> Scene.walk |> Scene.password |> string

let job2 () =
    // input |> String.join "" |> string
    raise (System.NotImplementedException())
