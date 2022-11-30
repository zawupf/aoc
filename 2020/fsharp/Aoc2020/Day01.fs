module Day01

open Utils

let pairWithItem items first =
    seq {
        if items |> (not << Seq.isEmpty) then
            for second in items do
                yield first, second
    }

let pairWithTuple tuples first =
    seq {
        if tuples |> (not << Seq.isEmpty) then
            for second, third in tuples do
                yield first, second, third
    }

let combine items' =
    let items = items' |> Seq.toArray

    items
    |> Seq.indexed
    |> Seq.collect (fun (index, first) ->
        first |> pairWithItem (items |> Seq.skip (index + 1)))

let combine2 items' =
    let items = items' |> Seq.toArray

    items
    |> Seq.indexed
    |> Seq.collect (fun (index, first) ->
        first |> pairWithTuple (items |> Seq.skip (index + 1) |> combine))

let job1 () =
    readInputLines "01"
    |> Seq.map int
    |> combine
    |> Seq.filter (fun (a, b) -> a + b = 2020)
    |> Seq.head
    |> (fun (a, b) -> a * b)
    |> string

let job2 () =
    readInputLines "01"
    |> Seq.map int
    |> combine2
    |> Seq.filter (fun (a, b, c) -> a + b + c = 2020)
    |> Seq.head
    |> (fun (a, b, c) -> a * b * c)
    |> string
