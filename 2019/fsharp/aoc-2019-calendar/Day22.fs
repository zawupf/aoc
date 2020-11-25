module Day22

open System
open Utils

let deck n =
    [| for i in 0 .. n - 1 -> i |]

let dealIntoNewStack cards = cards |> Array.rev

let beforeDealIntoNewStack length index = length - index - 1L

let cut n cards =
    let m =
        if n > 0 then n else (cards |> Array.length) + n
    (cards |> Array.skip m, cards |> Array.take m) ||> Array.append

let beforeCut n length index =
    let m =
        if n > 0 then n else length + n

    let j = index - m
    if j < 0 then length + j else j

let dealWithIncrement n cards =
    let l = cards |> Array.length
    cards |> Array.permute (fun i -> i * n % l)

let dealWithIncrement2 n cards =
    let l = cards |> Array.length
    cards
    |> Array.map (fun i -> i, i * n, i * n / l, i * n % l)
    |> Array.permute (fun i -> i * n % l)

let beforeDealWithIncrement n length index = 0

let (|DealIntoNewStack|Cut|DealWithIncrement|) (line: string) =
    if line = "deal into new stack" then DealIntoNewStack
    else if line.StartsWith("cut ") then Cut(line.Substring(4) |> int)
    else if line.StartsWith("deal with increment ") then DealWithIncrement(line.Substring(20) |> int)
    else failwith "Invalid input"

let shuffle n lines =
    lines
    |> Seq.fold (fun cards ->
        function
        | DealIntoNewStack -> dealIntoNewStack cards
        | Cut n -> cut n cards
        | DealWithIncrement n -> dealWithIncrement n cards) (deck n)

let job1() =
    readInputLines "22"
    |> shuffle 10007
    |> Array.findIndex (fun i -> i = 2019)
    |> string

let job2() =
    raise (NotImplementedException())
    readInputLines "22" |> string
