#load "Utils.fsx"

type Card =
    | Card_A
    | Card_K
    | Card_Q
    | Card_J
    | Card_T
    | Card_9
    | Card_8
    | Card_7
    | Card_6
    | Card_5
    | Card_4
    | Card_3
    | Card_2

module Card =
    let strength =
        function
        | Card_A -> (13, 13)
        | Card_K -> (12, 12)
        | Card_Q -> (11, 11)
        | Card_J -> (10, 0)
        | Card_T -> (9, 9)
        | Card_9 -> (8, 8)
        | Card_8 -> (7, 7)
        | Card_7 -> (6, 6)
        | Card_6 -> (5, 5)
        | Card_5 -> (4, 4)
        | Card_4 -> (3, 3)
        | Card_3 -> (2, 2)
        | Card_2 -> (1, 1)

    let parse =
        function
        | 'A' -> Card_A
        | 'K' -> Card_K
        | 'Q' -> Card_Q
        | 'J' -> Card_J
        | 'T' -> Card_T
        | '9' -> Card_9
        | '8' -> Card_8
        | '7' -> Card_7
        | '6' -> Card_6
        | '5' -> Card_5
        | '4' -> Card_4
        | '3' -> Card_3
        | '2' -> Card_2
        | c -> failwithf "Invalid card: %A" c

type Hand = Hand of Card[]

module Hand =
    [<RequireQualifiedAccess>]
    type Type =
        | ``Five of a kind``
        | ``Four of a kind``
        | ``Full house``
        | ``Three of a kind``
        | ``Two pair``
        | ``One pair``
        | ``High card``

    let strength =
        function
        | Type.``Five of a kind`` -> 7
        | Type.``Four of a kind`` -> 6
        | Type.``Full house`` -> 5
        | Type.``Three of a kind`` -> 4
        | Type.``Two pair`` -> 3
        | Type.``One pair`` -> 2
        | Type.``High card`` -> 1

    let parse = Utils.String.toCharArray >> Array.map Card.parse >> Hand

    let typeOf (Hand hand) =
        hand
        |> Array.countBy id
        |> Array.map snd
        |> Array.sortDescending
        |> function
            | [| 5 |] -> Type.``Five of a kind``
            | [| 4; 1 |] -> Type.``Four of a kind``
            | [| 3; 2 |] -> Type.``Full house``
            | [| 3; 1; 1 |] -> Type.``Three of a kind``
            | [| 2; 2; 1 |] -> Type.``Two pair``
            | [| 2; 1; 1; 1 |] -> Type.``One pair``
            | [| 1; 1; 1; 1; 1 |] -> Type.``High card``
            | _ -> failwithf "Invalid hand: %A" hand

    let cards (Hand cards) = cards

    let sortKey1 hand =
        hand |> typeOf |> strength,
        hand |> cards |> Array.map (Card.strength >> fst)

    let replaceJoker (Hand cards) =
        cards
        |> Array.filter (fun card -> card <> Card_J)
        |> Array.countBy id
        |> Array.sortByDescending snd
        |> Array.tryHead
        |> function
            | Some(card, _) ->
                cards |> Array.map (fun c -> if c = Card_J then card else c)
            | None -> Array.create 5 Card_A

    let sortKey2 hand =
        let hand' = replaceJoker hand |> Hand

        hand' |> typeOf |> strength,
        hand |> cards |> Array.map (Card.strength >> snd)

let parse line =
    match line |> Utils.String.split ' ' with
    | [| hand; bid |] -> Hand.parse hand, int bid
    | _ -> failwithf "Invalid line: %A" line

let part1 input = //
    input
    |> Array.map parse
    |> Array.sortBy (fst >> Hand.sortKey1)
    |> Array.mapi (fun i (_, bid) -> (i + 1) * bid)
    |> Array.sum

let part2 input = //
    input
    |> Array.map parse
    |> Array.sortBy (fst >> Hand.sortKey2)
    |> Array.mapi (fun i (_, bid) -> (i + 1) * bid)
    |> Array.sum



let testInput =
    [|
        """
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
"""
    |]
    |> (Array.map Utils.String.toLines)

Utils.Test.run "Test part 1" 6440 (fun () -> part1 testInput[0])
Utils.Test.run "Test part 2" 5905 (fun () -> part2 testInput[0])



let input = Utils.readInputLines "07"

let getDay07_1 () = part1 input

let getDay07_2 () = part2 input

Utils.Test.run "Part 1" 249748283 getDay07_1
Utils.Test.run "Part 2" 248029057 getDay07_2
