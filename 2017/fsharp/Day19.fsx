#load "Utils.fsx"
open Utils

let walk input =
    let diagram =
        input
        |> String.split '\n'
        |> Array.filter (fun line ->
            not <| System.String.IsNullOrWhiteSpace line)
        |> Array.map String.toCharArray

    let item (x, y) =
        diagram
        |> Array.tryItem y
        |> Option.bind (fun row -> row |> Array.tryItem x)
        |> Option.defaultValue ' '

    let start = diagram[0] |> Array.findIndex ((=) '|'), 0

    let rec loop (x, y) (dx, dy) locations steps =
        let next = x + dx, y + dy

        match item next with
        | ' ' -> locations |> List.rev |> String.ofChars, steps
        | '|'
        | '-' -> loop next (dx, dy) locations (steps + 1)
        | '+' ->
            let nextDir =
                let nx, ny = next

                match dx, dy with
                | 0, _ -> [ 1, 0; -1, 0 ]
                | _, 0 -> [ 0, 1; 0, -1 ]
                | _ -> unreachable ()
                |> List.filter (fun (dx, dy) -> item (nx + dx, ny + dy) <> ' ')
                |> List.exactlyOne

            loop next nextDir locations (steps + 1)
        | c -> loop next (dx, dy) (c :: locations) (steps + 1)

    loop start (0, 1) [] 1

let part1 input = input |> walk |> fst

let part2 input = input |> walk |> snd

let day = __SOURCE_FILE__[3..4]
let input = readInputExact day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput = [|
    """
     |
     |  +--+
     A  |  C
 F---|----E|--+
     |  |  |  D
     +B-+  +--+
"""
|]

Test.run "Test 1" "ABCDEF" (fun () -> part1 testInput[0])
Test.run "Test 2" 38 (fun () -> part2 testInput[0])

Test.run "Part 1" "VEBTPXCHLI" solution1
Test.run "Part 2" 18702 solution2

#load "_benchmark.fsx"
