#load "Utils.fsx"
open Utils

let normalize (input: string) = input.Replace("\t", " ")

let reallocCount banks =
    let n = Array.length banks

    let findBankWithMostBlocks banks =
        let m = banks |> Array.max
        banks |> Array.findIndex (fun x -> x = m)

    let toKey banks = banks |> Array.map string |> String.concat " "

    let cache = Dictionary<string, int>()

    let rec loop count =
        let key = banks |> toKey

        if cache.TryAdd(key, count) then
            let i = findBankWithMostBlocks banks
            let blocks = banks[i]
            let nAll, nPartial = blocks / n, blocks % n
            banks[i] <- 0

            for j = 0 to n - 1 do
                banks[j] <- banks[j] + nAll

            for j = 1 to nPartial do
                let idx = (i + j) % n
                banks[idx] <- banks[idx] + 1

            loop (count + 1)
        else
            count, count - cache[key]

    loop 0

let part1 input =
    input |> normalize |> String.parseInts ' ' |> reallocCount |> fst

let part2 input =
    input |> normalize |> String.parseInts ' ' |> reallocCount |> snd

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
0 2 7 0
"""
    |]
    |> Array.map String.trim

Test.run "Test 1" 5 (fun () -> part1 testInput[0])
Test.run "Test 2" 4 (fun () -> part2 testInput[0])

Test.run "Part 1" 14029 solution1
Test.run "Part 2" 2765 solution2

#load "_benchmark.fsx"
