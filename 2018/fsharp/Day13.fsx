#load "Utils.fsx"
open Utils

type Direction =
    | Up
    | Down
    | Left
    | Right

type Track =
    | Empty
    | Vertical
    | Horizontal
    | Intersection
    | CurveForward
    | CurveBackward

type Turn =
    | TurnLeft
    | GoStraight
    | TurnRight

type Cart = { position: int * int; direction: Direction; turn: Turn }

let parse lines =
    let parseTrack c =
        match c with
        | ' ' -> Empty
        | '|' -> Vertical
        | '-' -> Horizontal
        | '+' -> Intersection
        | '/' -> CurveForward
        | '\\' -> CurveBackward
        | '>'
        | '<' -> Horizontal
        | '^'
        | 'v' -> Vertical
        | _ -> failwithf "Unknown track: %c" c

    let parseDirection c =
        match c with
        | '>' -> Some Right
        | '<' -> Some Left
        | '^' -> Some Up
        | 'v' -> Some Down
        | _ -> None

    let parseCart c (x, y) =
        match parseDirection c with
        | Some direction ->
            Some { position = x, y; direction = direction; turn = TurnLeft }
        | None -> None

    let maxLineLength = lines |> Array.map String.length |> Array.max

    let fields =
        lines
        |> Array.map (fun line ->
            line.PadRight maxLineLength |> String.toCharArray)

    let carts =
        fields
        |> Array.mapi (fun y row ->
            row |> Array.mapi (fun x c -> parseCart c (x, y)))
        |> Array.collect (Array.choose id)

    let tracks =
        fields |> Array.map (Array.map parseTrack) |> Array.transpose |> array2D

    carts, tracks

let moveCart (tracks: Track[,]) cart =
    let { position = x, y; direction = direction; turn = turn } = cart

    let x', y' =
        match direction with
        | Up -> x, y - 1
        | Down -> x, y + 1
        | Left -> x - 1, y
        | Right -> x + 1, y

    let track' = tracks[x', y']

    let direction' =
        match track' with
        | Vertical
        | Horizontal -> direction
        | Intersection ->
            match direction, turn with
            | Up, TurnLeft -> Left
            | Up, TurnRight -> Right
            | Down, TurnLeft -> Right
            | Down, TurnRight -> Left
            | Left, TurnLeft -> Down
            | Left, TurnRight -> Up
            | Right, TurnLeft -> Up
            | Right, TurnRight -> Down
            | _, GoStraight -> direction
        | CurveForward ->
            match direction with
            | Up -> Right
            | Down -> Left
            | Left -> Down
            | Right -> Up
        | CurveBackward ->
            match direction with
            | Up -> Left
            | Down -> Right
            | Left -> Up
            | Right -> Down
        | _ -> failwithf "Unknown track: %A" track'

    let turn' =
        match track' with
        | Intersection ->
            match turn with
            | TurnLeft -> GoStraight
            | GoStraight -> TurnRight
            | TurnRight -> TurnLeft
        | _ -> turn

    { position = x', y'; direction = direction'; turn = turn' }

let part1 input =
    let carts, tracks = parse input

    let rec loop carts =
        carts
        |> Array.sortInPlaceBy (fun cart ->
            let x, y = cart.position
            y, x)

        let collision =
            carts
            |> Array.indexed
            |> Array.fold
                (fun collision (i, cart) ->
                    match collision with
                    | Some _ -> collision
                    | None ->
                        carts[i] <- moveCart tracks cart

                        let collisions =
                            carts
                            |> Array.countBy (fun cart -> cart.position)
                            |> Array.filter (fun (_, count) -> count > 1)
                            |> Array.map fst

                        match collisions with
                        | [||] -> None
                        | [| collision |] -> Some collision
                        | _ -> unreachable ())
                None

        match collision with
        | None -> loop carts
        | Some(x, y) -> sprintf "%d,%d" x y

    loop carts

let part2 input =
    let carts, tracks = parse input

    let rec loop carts =
        carts
        |> Array.sortInPlaceBy (fun cart ->
            let x, y = cart.position
            y, x)

        let collisions =
            carts
            |> Array.indexed
            |> Array.fold
                (fun collisions (i, cart) ->
                    match collisions |> List.contains cart.position with
                    | true -> collisions
                    | false ->
                        carts[i] <- moveCart tracks cart

                        carts
                        |> Array.countBy (fun cart -> cart.position)
                        |> Array.filter (fun (_, count) -> count > 1)
                        |> Array.map fst
                        |> Array.toList)
                []

        let carts =
            carts
            |> Array.filter (fun cart ->
                collisions |> List.contains cart.position |> not)

        match carts with
        | [||] -> unreachable ()
        | [| cart |] -> cart.position ||> sprintf "%d,%d"
        | _ -> loop carts

    loop carts

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
/->-\
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/
"""
        """
/>-<\
|   |
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/
"""
    |]
    |> Array.map (String.trim >> String.split '\n')

Test.run "Test 1" "7,3" (fun () -> part1 testInput[0])
Test.run "Test 2" "6,4" (fun () -> part2 testInput[1])

Test.run "Part 1" "76,108" solution1
Test.run "Part 2" "2,84" solution2

#load "_benchmark.fsx"
