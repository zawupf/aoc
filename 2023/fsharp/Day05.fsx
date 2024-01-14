#load "Utils.fsx"

type Range = Range of int64 * int64

module Range =
    let contains v (Range(a, b)) = v >= a && v <= b

    let start (Range(a, _)) = a

    let add offset (Range(a, b)) = Range(a + offset, b + offset)

type RangeMap = { Source: Range; Offset: int64 }

module RangeMap =
    let create dst src len = {
        Source = Range(src, src + len - 1L)
        Offset = dst - src
    }

    let tryMapValue v map =
        if map.Source |> Range.contains v then
            Some(v + map.Offset)
        else
            None

    let mapValue v =
        Array.tryPick (tryMapValue v) >> Option.defaultValue v

    let tryMapRange (todo, done') map =
        let move = Range.add map.Offset
        let (Range(ma, mb)) = map.Source

        todo
        |> List.fold
            (fun (todo', done') r ->
                let (Range(a, b)) = r

                let (outsideRanges, insideRange) =
                    if a >= ma && b <= mb then
                        [], [ r ]
                    elif b < ma then
                        [ r ], []
                    elif a > mb then
                        [ r ], []
                    elif a < ma && b <= mb then
                        [ Range(a, ma - 1L) ], [ Range(ma, b) ]
                    elif a >= ma && b > mb then
                        [ Range(mb + 1L, b) ], [ Range(a, mb) ]
                    elif a < ma && b > mb then
                        [ Range(a, ma - 1L); Range(mb + 1L, b) ],
                        [ map.Source ]
                    else
                        failwith "Case missed? 🤔"

                outsideRanges @ todo', (insideRange |> List.map move) @ done')
            ([], done')

    let mapRanges rs maps =
        maps |> Array.fold tryMapRange (rs, []) ||> (@)


open Utils.FancyPatterns

let parse input =
    let chunks =
        input
        |> Utils.String.split "\n\n"
        |> Array.toList
        |> List.map Utils.String.toLines

    let seeds =
        match chunks.Head[0] with
        | Regex @"((?: *\d+)+)" [ values ] ->
            values |> Utils.String.parseInt64s ' '
        | line -> failwithf "Invalid seeds: %A" line

    let maps =
        chunks.Tail
        |> List.map (fun lines ->
            lines
            |> Array.skip 1
            |> Array.map (fun line ->
                match line |> Utils.String.parseInt64s ' ' with
                | [| dst; src; len |] -> RangeMap.create dst src len
                | _ -> failwithf "Invalid range map: %A" line))

    seeds, maps

let part1 input = //
    let seeds, maps = parse input

    seeds
    |> Array.map (fun seed -> maps |> List.fold RangeMap.mapValue seed)
    |> Array.min

let part2 input = //
    let seeds, maps = parse input

    let seeds =
        seeds
        |> List.ofArray
        |> List.chunkBySize 2
        |> List.map (function
            | [ start; length ] -> Range(start, start + length - 1L)
            | r -> failwithf "Invalid range: %A" r)

    maps
    |> List.fold RangeMap.mapRanges seeds
    |> List.map Range.start
    |> List.min



let testInput = [|
    """
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
"""
|]

Utils.Test.run "Test part 1" 35L (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 46L (fun () -> part2 testInput[0])



let input = Utils.readInputText "05"

let getDay05_1 () = part1 input

let getDay05_2 () = part2 input

Utils.Test.run "Part 1" 175622908L getDay05_1
Utils.Test.run "Part 2" 5200543L getDay05_2
