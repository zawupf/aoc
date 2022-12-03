module Day03

open Utils

let findDoubleItem rucksack =
    rucksack
    |> Seq.splitInto 2
    |> Seq.map Set.ofSeq
    |> Set.intersectMany
    |> Seq.exactlyOne

let findBadgeItem rucksacks =
    rucksacks |> Seq.map Set.ofSeq |> Set.intersectMany |> Seq.exactlyOne

let priority c =
    [| yield! [| 'a' .. 'z' |]; yield! [| 'A' .. 'Z' |] |]
    |> Array.findIndex ((=) c)
    |> (+) 1

let sumOfDoubleItemPrios rucksacks =
    rucksacks |> Array.sumBy (findDoubleItem >> priority)

let sumOfBadgeItemPrios rucksacks =
    rucksacks |> Array.chunkBySize 3 |> Array.sumBy (findBadgeItem >> priority)

let input = readInputLines "03"

let job1 () = input |> sumOfDoubleItemPrios |> string

let job2 () = input |> sumOfBadgeItemPrios |> string
