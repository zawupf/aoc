module Day05

open Utils

let computePassword id =
    Seq.unfold (fun i -> Some(Md5.ofString (id + string i), i + 1)) 0
    |> Seq.filter (String.startsWith "00000")
    |> Seq.take 8
    |> Seq.map (fun md5 -> md5.[5])
    |> Seq.toArray
    |> System.String
    |> (fun s -> s.ToLowerInvariant())

let computePassword2 id =
    let handled = System.Collections.Generic.HashSet<char>()

    let isValid pos = pos >= '0' && pos <= '7' && (handled.Contains pos |> not)

    Seq.unfold (fun i -> Some(Md5.ofString (id + string i), i + 1)) 0
    |> Seq.choose (fun md5 ->
        match md5.StartsWith("00000") with
        | true ->
            match md5.[5] with
            | pos when pos |> isValid ->
                handled.Add(pos) |> ignore
                Some(pos, md5.[6])
            | _ -> None
        | false -> None)
    |> Seq.take 8
    |> Seq.sortBy fst
    |> Seq.map snd
    |> Seq.toArray
    |> System.String
    |> (fun s -> s.ToLowerInvariant())

let input = readInputText "05"

let job1 () = input |> computePassword

let job2 () = input |> computePassword2
