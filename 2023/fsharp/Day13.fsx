#load "Utils.fsx"

[<TailCall>]
let rec mirror l f i j except delta =
    let i', j' = i - delta, j + delta

    let isValidRange = i' >= 0 && i' < l && j' >= 0 && j' < l

    if not isValidRange then
        match except with
        | None -> Some j
        | Some e when j <> e -> Some j
        | Some _ -> None
    else
        let ai = f i'
        let aj = f j'

        if ai = aj then mirror l f i j except (delta + 1) else None

let findMirror len getter except =
    seq { 0 .. len - 1 }
    |> Seq.pairwise
    |> Seq.map (fun (i, j) -> mirror len getter i j except 0)
    |> Seq.tryFind Option.isSome
    |> Option.flatten
    |> Option.defaultValue 0

let findColumnMirrorNote pattern except =
    findMirror
        (Array2D.length2 pattern)
        (fun x -> pattern[0.., x])
        (except |> Option.bind (fun e -> if e >= 100 then None else Some e))

let findRowMirrorNote pattern except =
    100
    * findMirror
        (Array2D.length1 pattern)
        (fun y -> pattern[y, 0..])
        (except
         |> Option.bind (fun e -> if e < 100 then None else Some(e / 100)))

let toPattern =
    Utils.String.toLines >> (Array.map Utils.String.toCharArray) >> array2D

let part1 input =
    (input |> Utils.String.trim).Split("\n\n")
    |> Array.map (fun block ->
        let pattern = block |> toPattern

        match findColumnMirrorNote pattern None with
        | 0 -> findRowMirrorNote pattern None
        | note -> note)
    |> Array.sum

let part2 input =
    (input |> Utils.String.trim).Split("\n\n")
    |> Array.map (fun block ->
        let pattern = block |> toPattern

        let previousValue =
            match findColumnMirrorNote pattern None with
            | 0 -> findRowMirrorNote pattern None
            | note -> note
            |> Some

        let xMin, xMax = 0, Array2D.length2 pattern - 1
        let yMin, yMax = 0, Array2D.length1 pattern - 1

        seq {
            for y in yMin..yMax do
                for x in xMin..xMax -> x, y
        }
        |> Seq.map (fun (x, y) ->
            pattern[y, x] <- if pattern[y, x] = '.' then '#' else '.'

            let result =
                match findColumnMirrorNote pattern previousValue with
                | 0 -> findRowMirrorNote pattern previousValue
                | note -> note

            pattern[y, x] <- if pattern[y, x] = '.' then '#' else '.'
            result)
        |> Seq.find ((<>) 0))
    |> Array.sum



let input = Utils.readInputText "13"

let getDay13_1 () = part1 input

let getDay13_2 () = part2 input

Utils.test_result "Part 1" 31877 getDay13_1
Utils.test_result "Part 2" 42996 getDay13_2



let testInput1 =
    """
#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#
"""

Utils.test_result "Test part 1" 405 (fun () -> part1 testInput1)
Utils.test_result "Test part 2" 400 (fun () -> part2 testInput1)
