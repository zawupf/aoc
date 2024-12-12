#load "Utils.fsx"

type Name = char
type Pos = Pos of int * int

type EdgeKey =
    | TopOfRow of int
    | BottomOfRow of int
    | LeftOfCol of int
    | RightOfCol of int

type EdgeValue = int
type Edge = EdgeKey * EdgeValue

type Region = { Name: Name; Fields: Pos Set }
type Grid = Name[][]

let parseGrid (input: string[]) : Grid =
    input |> Array.map (fun line -> line.ToCharArray())

let neighbors (Pos(x, y)) =
    seq {
        yield Pos(x + 1, y)
        yield Pos(x - 1, y)
        yield Pos(x, y + 1)
        yield Pos(x, y - 1)
    }

let isValidPos (grid: Grid) (Pos(x, y)) =
    x >= 0 && x < grid.[0].Length && y >= 0 && y < grid.Length

let scanRegion (grid: Grid) (Pos(x, y)) =
    let name = grid.[y].[x]

    let isValidName (Pos(x, y)) =
        isValidPos grid (Pos(x, y)) && grid.[y].[x] = name

    let prependTo state pos =
        if isValidName pos then pos :: state else state

    let withNextOf pos state =
        let (Pos(x, y)) = pos
        grid.[y].[x] <- '.'

        pos |> neighbors |> Seq.fold prependTo state

    let fields =
        Seq.unfold
            (fun state ->
                match state with
                | [] -> None
                | pos :: rest -> Some(pos, withNextOf pos rest))
            [ Pos(x, y) ]
        |> Set.ofSeq

    { Name = name; Fields = fields }

let scanRegions (_grid: Grid) : Region seq =
    let grid = _grid |> Array.map Array.copy

    seq {
        for y in 0 .. grid.Length - 1 do
            for x in 0 .. grid.[0].Length - 1 do
                if grid.[y].[x] <> '.' then
                    yield scanRegion grid (Pos(x, y))
    }

let area region = region.Fields.Count

let outside region pos = region.Fields.Contains pos |> not

let perimeter region =
    region.Fields
    |> Seq.sumBy (neighbors >> Seq.filter (outside region) >> Seq.length)

let edges region (Pos(x, y)) : Edge seq =
    seq {
        for Pos(x', y') in neighbors (Pos(x, y)) |> Seq.filter (outside region) do
            match x' - x, y' - y with
            | 1, 0 -> yield RightOfCol x, y
            | -1, 0 -> yield LeftOfCol x, y
            | 0, 1 -> yield BottomOfRow y, x
            | 0, -1 -> yield TopOfRow y, x
            | _ -> ()
    }

let gapCount values =
    values
    |> Seq.map snd
    |> Seq.sort
    |> Seq.pairwise
    |> Seq.sumBy (fun (a, b) -> b - a - 1 |> sign)

let sideCount region =
    region.Fields
    |> Seq.collect (edges region)
    |> Seq.groupBy fst
    |> Seq.sumBy (snd >> gapCount >> (+) 1)

let part1 input =
    input
    |> parseGrid
    |> scanRegions
    |> Seq.sumBy (fun r -> area r * perimeter r)

let part2 input =
    input
    |> parseGrid
    |> scanRegions
    |> Seq.sumBy (fun r -> area r * sideCount r)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
            AAAA
            BBCD
            BBCC
            EEEC
        """
        """
            OOOOO
            OXOXO
            OOOOO
            OXOXO
            OOOOO
        """
        """
            RRRRIICCFF
            RRRRIICCCF
            VVRRRCCFFF
            VVRCCCJFFF
            VVVVCJJCFE
            VVIVCCJJEE
            VVIIICJJEE
            MIIIIIJJEE
            MIIISIJEEE
            MMMISSJEEE
        """
        """
            EEEEE
            EXXXX
            EEEEE
            EXXXX
            EEEEE
        """
        """
            AAAAAA
            AAABBA
            AAABBA
            ABBAAA
            ABBAAA
            AAAAAA
        """
    |]
    |> Array.map Utils.String.toLines

Utils.Test.run "Test 1" 140 (fun () -> part1 testInput[0])
Utils.Test.run "Test 1" 772 (fun () -> part1 testInput[1])
Utils.Test.run "Test 1" 1930 (fun () -> part1 testInput[2])
Utils.Test.run "Test 2" 80 (fun () -> part2 testInput[0])
Utils.Test.run "Test 2" 436 (fun () -> part2 testInput[1])
Utils.Test.run "Test 2" 236 (fun () -> part2 testInput[3])
Utils.Test.run "Test 2" 368 (fun () -> part2 testInput[4])
Utils.Test.run "Test 2" 1206 (fun () -> part2 testInput[2])

Utils.Test.run "Part 1" 1381056 solution1
Utils.Test.run "Part 2" 834828 solution2
