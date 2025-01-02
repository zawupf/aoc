#load "Utils.fsx"
open Utils

let hasNoDuplicateWords passphrase =
    let words = passphrase |> String.split ' '
    let uniqueWords = words |> Array.distinct
    words.Length = uniqueWords.Length

let hasNoAnagrams passphrase =
    let chars =
        passphrase
        |> String.split ' '
        |> Array.map (fun w -> w |> String.toCharArray |> Array.sort)

    let uniqueChars = chars |> Array.distinct
    chars.Length = uniqueChars.Length

let part1 input =
    input
    |> Array.sumBy (fun passphrase ->
        if hasNoDuplicateWords passphrase then 1 else 0)

let part2 input =
    input
    |> Array.sumBy (fun passphrase -> if hasNoAnagrams passphrase then 1 else 0)

let day = __SOURCE_FILE__[3..4]
let input = readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput =
    [|
        """
aa bb cc dd ee
aa bb cc dd aa
aa bb cc dd aaa
"""
        """
abcde fghij
abcde xyz ecdab
a ab abc abd abf abj
iiii oiii ooii oooi oooo
oiii ioii iioi iiio
"""
    |]
    |> Array.map String.toLines

Test.run "Test 1" 2 (fun () -> part1 testInput[0])
Test.run "Test 2" 3 (fun () -> part2 testInput[1])

Test.run "Part 1" 337 solution1
Test.run "Part 2" 231 solution2

#load "_benchmark.fsx"
