#load "Utils.fsx"
open Utils

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

let simpleHash size input =
    input
    |> String.parseInts ","
    |> Array.map byte
    |> hash size
    |> Array.take 2
    |> Array.map int
    |> Array.reduce (*)

let knotHash input =
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
    |> Array.map (fun block -> block |> Array.reduce (^^^) |> _.ToString("x2"))
    |> String.join ""

let part1 input = input |> simpleHash 256

let part2 input = input |> knotHash

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput = [|
    "", "a2582a3a0e66e6e86e3812dcb672a272"
    "AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd"
    "1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d"
    "1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e"
|]

Test.run "Test 1" 12 (fun () -> simpleHash 5 "3,4,1,5")

for input, expected in testInput do
    Test.run $"Test 2 ({input})" expected (fun () -> part2 input)

Test.run "Part 1" 13760 solution1
Test.run "Part 2" "2da93395f1a6bb3472203252e3b17fe5" solution2

#load "_benchmark.fsx"
