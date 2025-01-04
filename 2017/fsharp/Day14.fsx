#load "Utils.fsx"
open Utils

let knotHash input =
    let hash size lengths =
        lengths
        |> Seq.fold
            (fun ((list: byte[]), (current, skip)) l ->
                let length = int l
                let start = current
                let stop = current + length - 1
                let m = length / 2 - 1

                for i in 0..m do
                    let a = (start + i) % size
                    let b = (stop - i) % size
                    let temp = list[a]
                    list[a] <- list[b]
                    list[b] <- temp

                list, (current + length + skip, skip + 1))
            ([| 0 .. size - 1 |] |> Array.map byte, (0, 0))
        |> fst

    let lengths =
        Array.concat [
            input |> String.toByteArray
            [| 17uy; 31uy; 73uy; 47uy; 23uy |]
        ]

    seq {
        for _ in 1..64 do
            yield! lengths
    }
    |> hash 256
    |> Array.chunkBySize 16
    |> Array.map (fun block -> block |> Array.reduce (^^^) |> _.ToString("b8"))
    |> String.join ""

let initGrid input =
    [| 0..127 |]
    |> Array.map (fun i ->
        sprintf "%s-%d" input i
        |> knotHash
        |> String.toCharArray
        |> Array.map (fun c -> c - '0' |> int))

let countUsed grid = grid |> Array.sumBy Array.sum

let countRegions grid =
    let isUsed (x, y) =
        grid |> Array.item y |> Array.item x = 1

    let clear (x, y) =
        grid |> Array.item y |> Array.set <|| (x, 0)

    let isValid (x, y) = x >= 0 && x < 128 && y >= 0 && y < 128

    let markRegion (x, y) =
        let rec loop cells n =
            match cells with
            | [] -> n
            | (x, y) :: rest when isValid (x, y) && isUsed (x, y) ->
                clear (x, y)

                loop
                    ((x - 1, y)
                     :: (x + 1, y)
                     :: (x, y - 1)
                     :: (x, y + 1)
                     :: rest)
                    (n + 1)
            | _ :: rest -> loop rest n

        loop [ (x, y) ] 0

    seq { 0..127 }
    |> Seq.collect (fun y -> seq { 0..127 } |> Seq.map (fun x -> x, y))
    |> Seq.sumBy (markRegion >> sign)

let part1 input = input |> initGrid |> countUsed

let part2 input = input |> initGrid |> countRegions

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput = [| "flqrgnkx" |] |> Array.map String.trim

Test.run "Test 1" 8108 (fun () -> part1 testInput[0])
Test.run "Test 2" 1242 (fun () -> part2 testInput[0])

Test.run "Part 1" 8214 solution1
Test.run "Part 2" 1093 solution2

#load "_benchmark.fsx"
