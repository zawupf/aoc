module Day17

open Utils

type Shape =
    | HorizontalBar = 0
    | Cross = 1
    | Boomarang = 2
    | VerticalBar = 3
    | Block = 4

type Sprite = string array

type Chamber() =
    let array = ResizeArray<string>()
    let mutable offset = 0L
    do array.Capacity <- Chamber.CapacityBase + 10

    static member private CapacityBase = 100000

    member internal _.Height = offset + int64 array.Count

    member _.Offset
        with private set value = offset <- value

    member _.Item
        with internal get rowIndex = array[int (rowIndex - offset)]
        and internal set rowIndex row = array[int (rowIndex - offset)] <- row

    member _.Last = array[array.Count - 1]

    member internal _.Add(row) =
        array.Add(row)

        if array.Count > Chamber.CapacityBase then
            offset <- offset + int64 array.Count - 1000L
            array.RemoveRange(0, array.Count - 1000)
            assert (array.Capacity = Chamber.CapacityBase + 10)

module Shape =
    let next shape = enum<Shape> ((int shape + 1) % 5)

    let seq first =
        first |> Seq.unfold (fun shape -> Some(shape, next shape))

    let createSprite shape : Sprite =
        match shape with
        | Shape.HorizontalBar -> [| "####" |]
        | Shape.Cross -> [| ".#."; "###"; ".#." |]
        | Shape.Boomarang -> [| "###"; "..#"; "..#" |]
        | Shape.VerticalBar -> [| "#"; "#"; "#"; "#" |]
        | Shape.Block -> [| "##"; "##" |]
        | _ -> failwith "Shape out of range"

module Sprite =
    let height sprite = sprite |> Array.length
    let width sprite = sprite |> Array.head |> String.length
    let get (x, y) (sprite: Sprite) = sprite[y][x]

module Chamber =
    let width = 7

    let height (chamber: Chamber) = chamber.Height

    let get (x, y) (chamber: Chamber) =
        match x, y with
        | x, y when x < 0 || x >= width || y < 0L -> '#'
        | _, y when y >= height chamber -> '.'
        | x, y -> chamber[y][x]

    let rec getRow y (chamber: Chamber) =
        if y < height chamber then
            chamber[y]
        else
            chamber.Add(String.replicate width ".")
            getRow y chamber

    let add (x, y) (sprite: Sprite) (chamber: Chamber) =
        for dy, line in sprite |> Seq.indexed do
            let row = getRow (y + int64 dy) chamber |> String.toCharArray

            for dx, char in line |> Seq.indexed do
                if char = '#' then
                    row[x + dx] <- char

            chamber[y + int64 dy] <- String.ofChars row

        chamber

    let isColliding (x, y) sprite chamber =
        seq {
            for dy in 0 .. Sprite.height sprite - 1 do
                for dx in 0 .. Sprite.width sprite - 1 -> dx, dy
        }
        |> Seq.exists (fun (dx, dy) ->
            match
                Sprite.get (dx, dy) sprite, get (x + dx, y + int64 dy) chamber
            with
            | '#', '#' -> true
            | _, _ -> false)

    let push direction chamber rock =
        let (x, y), sprite = rock

        let x' =
            match direction with
            | '<' -> x - 1
            | '>' -> x + 1
            | _ -> failwith "Invalid direction"

        match isColliding (x', y) sprite chamber with
        | true -> rock
        | false -> (x', y), sprite

    let drop (directions: string) (chamber, dirIndex) shape =
        let sprite = Shape.createSprite shape

        let rec loop (x, y) chamber dirIndex =
            let (x, y), _ = push directions[dirIndex] chamber ((x, y), sprite)
            let dirIndex = (dirIndex + 1) % String.length directions
            let y' = y - 1L

            match isColliding (x, y') sprite chamber with
            | false -> loop (x, y') chamber dirIndex
            | true -> add (x, y) sprite chamber, dirIndex

        let x, y = 2, height chamber + 3L
        loop (x, y) chamber dirIndex

let chamberHeightAfter rockCount directions =
    let rec loop n shape chamber dirIndex =
        match n with
        | 0L -> chamber, dirIndex
        | _ ->
            let chamber, dirIndex =
                Chamber.drop directions (chamber, dirIndex) shape

            if chamber.Last = "#######" then
                printfn "Full row at y=%d" chamber.Height

            loop (n - 1L) (Shape.next shape) chamber dirIndex

    loop rockCount Shape.HorizontalBar (Chamber()) 0 |> (fst >> Chamber.height)

let input = readInputText "17"

let job1 () =
    // input |> chamberHeightAfter 2022L |> string
    input |> chamberHeightAfter 100000L |> string

let job2 () =
    // chamberHeightAfter
    //     1000000L
    //     // 1000000000000L
    //     ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    // |> string
    raise (System.NotImplementedException())
