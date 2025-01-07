module Day14

open Utils

let hashes1 salt =
    Seq.initInfinite (fun i -> i, Md5.ofString (salt + (string i)))

let hashes2 salt =
    Seq.initInfinite (fun i ->
        let hash =
            { 1..2016 }
            |> Seq.fold
                (fun hash _ -> Md5.ofString (hash.ToLowerInvariant()))
                (Md5.ofString (salt + (string i)))

        i, hash)

let keys (hashes: (int * string) seq) =
    let tryFindThree string =
        string
        |> Seq.windowed 3
        |> Seq.tryPick (function
            | [| a; b; c |] ->
                match a = b && b = c with
                | true -> Some a
                | false -> None
            | _ -> failwith "Invalid chunk")

    seq {
        for chunk in hashes |> Seq.windowed 1001 do
            let hash = chunk |> Array.head

            match hash |> snd |> tryFindThree with
            | Some c ->
                let s = System.String(c, 5)

                if
                    chunk
                    |> Seq.skip 1
                    |> Seq.exists (fun (_, hash) -> hash.Contains(s))
                then
                    yield hash
            | _ -> ()
    }

let input = readInputText "14"

let job1 () = input |> hashes1 |> keys |> Seq.item 63 |> fst |> string

let job2 () = input |> hashes2 |> keys |> Seq.item 63 |> fst |> string
