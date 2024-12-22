#load "Utils.fsx"

let parse lines = lines |> Array.map uint32

let mix value = (^^^) value
let purge = (&&&) 0xFFFFFFu

let transform fn secret = secret |> mix (fn secret) |> purge

let secrets seed =
    Seq.unfold
        (fun secret ->
            let next =
                secret
                |> transform (fun s -> s <<< 6)
                |> transform (fun s -> s >>> 5)
                |> transform (fun s -> s <<< 11)

            Some(next, next))
        seed

let collectStats n seed =
    let toDigit secret = secret % 10u |> int

    seed
    |> secrets
    |> Seq.take n
    |> Seq.map toDigit
    |> Seq.fold
        (fun (stats, (previous, b, c, d)) current ->
            // Add offset to avoid negative numbers
            // Zero indicates that the value is not available
            let a = previous - current + 10

            match d with
            | 0 -> stats, (current, a, b, c)
            | _ ->
                let key = a <<< 24 ||| (b <<< 16) ||| (c <<< 8) ||| d

                stats |> Utils.Dictionary.tryAdd key current,
                (current, a, b, c))
        (Utils.Dictionary<int, int>(), (seed |> toDigit, 0, 0, 0))
    |> fst

let part1 input =
    input
    |> parse
    |> Array.sumBy (fun seed -> seed |> secrets |> Seq.item 1999 |> uint64)

let part2 input =
    let stats = input |> parse |> Array.map (collectStats 2000)

    let result =
        stats
        |> Array.collect (fun s -> s.Keys |> Seq.toArray)
        |> Array.distinct
        |> Array.map (fun key ->
            stats
            |> Array.sumBy (
                Utils.Dictionary.tryGetValue key >> Option.defaultValue 0
            ))
        |> Array.max

    result

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 18317943467UL solution1
Utils.Test.run "Part 2" 2018 solution2

#load "_benchmark.fsx"
