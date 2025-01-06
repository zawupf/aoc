#load "Utils.fsx"
open System

type Id = Id of int

type Number = Number of int

type Card = { Id: Id; Winning: Set<Number>; Numbers: Set<Number> }

module Card =
    open Utils.FancyPatterns

    let parse line =
        let toNumberSet =
            Utils.String.parseInts ' ' >> Array.map Number >> Set.ofArray

        match line with
        | Regex @"Card +(\d+):(.+)\|(.+)" [ Int id; winning; numbers ] -> {
            Id = Id id
            Winning = winning |> toNumberSet
            Numbers = numbers |> toNumberSet
          }
        | _ -> failwithf "Invalid card: %A" line

    let winCount card = Set.intersect card.Winning card.Numbers |> Set.count

let part1 input = //
    let points =
        function
        | n when n > 0 -> 1 <<< (n - 1)
        | _ -> 0

    input |> Array.sumBy (Card.parse >> Card.winCount >> points)

let part2 input = //
    let cardCounts = Array.create (input |> Array.length) 1

    let increment i n =
        match n with
        | n when n > 0 ->
            [ i + 1 .. i + n ]
            |> List.iter (fun j ->
                cardCounts
                |> Array.tryItem j
                |> Option.iter (fun m -> cardCounts[j] <- m + cardCounts[i]))
        | 0 -> ()
        | _ -> failwith "win count must not be negative"

        cardCounts[i]

    input
    |> Array.mapi (fun i line ->
        increment i (line |> (Card.parse >> Card.winCount)))
    |> Array.sum



let testInput = [|
    """
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
"""
    |> Utils.String.toLines
|]

Utils.Test.run "Test part 1" 13 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 30 (fun () -> part2 testInput[0])



let input = Utils.readInputLines "04"

let getDay04_1 () = part1 input

let getDay04_2 () = part2 input

Utils.Test.run "Part 1" 21138 getDay04_1
Utils.Test.run "Part 2" 7185540 getDay04_2
