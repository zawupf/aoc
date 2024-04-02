#load "Utils.fsx"

type Direction =
    | North
    | East
    | South
    | West

module Direction =
    let opposite =
        function
        | North -> South
        | South -> North
        | East -> West
        | West -> East

type Connection = Connection of Direction * Direction

module Connection =
    let create dir1 dir2 =
        if dir1 <> dir2 then
            Connection(min dir1 dir2, max dir1 dir2)
        else
            failwithf "Invalid connection: %A %A" dir1 dir2

    let other dir (Connection(dir1, dir2)) =
        if dir = dir1 then dir2
        elif dir = dir2 then dir1
        else failwithf "Invalid direction: %A" dir

    let contains dir (Connection(dir1, dir2)) = dir = dir1 || dir = dir2

type Connection with
    static member Create(dir1, dir2) = Connection.create dir1 dir2

type Tile =
    | Pipe of Connection
    | Ground

type Grid = Grid of Tile[,]

module Grid =
    let tryItem y x (Grid(tiles)) =
        if
            y >= 0
            && y < (tiles |> Array2D.length1)
            && x >= 0
            && x < (tiles |> Array2D.length2)
        then
            Some(tiles[y, x])
        else
            None

    let parse lines : Grid =
        let tiles =
            lines
            |> Array.mapi (fun y line ->
                line
                |> Utils.String.toCharArray
                |> Array.mapi (fun x c ->
                    match c with
                    | '|' -> Pipe(Connection.create North South)
                    | '-' -> Pipe(Connection.create East West)
                    | 'L' -> Pipe(Connection.create North East)
                    | 'J' -> Pipe(Connection.create North West)
                    | '7' -> Pipe(Connection.create South West)
                    | 'F' -> Pipe(Connection.create South East)
                    | '.' -> Ground
                    | 'S' -> resolveStart y x lines
                    | _ -> failwithf "Invalid tile character: %c" c))
            |> array2D

        Grid tiles

let part1 input = //
    0

let part2 input = //
    0



let testInput =
    [|
        """
.-L|F7.
.7S-7|.
.L|7||.
.-L-J|.
.L|-JF.
"""
        """
.7-F7-.
..FJ|7.
.SJLL7.
.|F--J.
.LJ.LJ.
"""
        """
...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
"""
    |]
    |> (Array.map Utils.String.toLines)

Utils.Test.run "Test part 1" 4 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 1" 8 (fun () -> part1 testInput[1])
Utils.Test.run "Test part 2" 1 (fun () -> part2 testInput[0])
Utils.Test.run "Test part 2" 1 (fun () -> part2 testInput[1])
Utils.Test.run "Test part 2" 4 (fun () -> part2 testInput[2])



let input = Utils.readInputLines "10"

let getDay10_1 () = part1 input

let getDay10_2 () = part2 input

Utils.Test.run "Part 1" 6697 getDay10_1
Utils.Test.run "Part 2" 423 getDay10_2
