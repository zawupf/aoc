module Day20

open Utils

type Range = uint * uint

module Range =
    let init a b =
        assert (a < b)
        if a < b then a, b else b, a

    let length (a: uint32, b: uint32) = uint64 b - uint64 a + 1UL

    let contains value (a, b) = value >= a && value <= b

    let tryMerge (a, b) (c, d) =
        let range = init (min a c) (max b d)
        let newLength = length range
        let oldLength = length (a, b) + length (c, d)
        if newLength <= oldLength then Some range else None

    let rec combine range ranges =
        let resultRange, resultRanges =
            ranges
            |> List.fold
                (fun (newRange, rs) r ->
                    match tryMerge newRange r with
                    | Some mergedRange -> mergedRange, rs
                    | None -> newRange, r :: rs)
                (range, [])

        if range = resultRange && ranges = (List.rev resultRanges) then
            range, ranges
        else
            combine resultRange resultRanges


    let parse =
        function
        | Regex @"^(\d+)-(\d+)$" [ UInt a; UInt b ] -> init a b
        | line -> failwith $"Invalid range: %s{line}"

let findFirstOutOfRange ranges =
    let rec loop value ranges =
        match ranges with
        | [] -> value
        | _ ->
            match ranges |> List.partition (Range.contains value) with
            | [], _ -> value
            | containing, remaining ->
                let nextValue = containing |> List.maxBy snd |> snd |> (+) 1u
                loop nextValue remaining

    loop 0u ranges

let allowedIpsCount ranges =
    let rec loop result ranges =
        match ranges with
        | [] -> result
        | range :: ranges ->
            let range, ranges = Range.combine range ranges
            loop (range :: result) ranges

    loop [] ranges
    |> List.sumBy (Range.length >> uint32)
    |> (-) System.UInt32.MaxValue
    |> (+) 1u

let input = readInputLines "20" |> Array.toList |> List.map Range.parse

let job1 () = input |> findFirstOutOfRange |> string

let job2 () = input |> allowedIpsCount |> string
