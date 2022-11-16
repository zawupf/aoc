module Day07

open Utils

let isAbba a b c d = a = d && b = c && a <> b
let isAba a b c = a = c && a <> b

let containsAbba string =
    string
    |> Seq.windowed 4
    |> Seq.exists (function
        | [| a; b; c; d |] when isAbba a b c d -> true
        | _ -> false)

let findAllAbas string =
    string
    |> Seq.windowed 3
    |> Seq.fold
        (fun abs ->
            function
            | [| a; b; c |] when isAba a b c -> abs |> Set.add (a, b)
            | _ -> abs)
        Set.empty

let findAllBabs = findAllAbas

let splitParts (ip: string) =
    ip.Split([| '['; ']' |])
    |> Array.chunkBySize 2
    |> Array.fold
        (fun (supernetParts, hypernetParts) chunk ->
            match chunk with
            | [| supernet; hypernet |] ->
                supernet :: supernetParts, hypernet :: hypernetParts
            | [| supernet |] -> supernet :: supernetParts, hypernetParts
            | _ -> failwith "Internal error")
        ([], [])

let isTlsSupported ip =
    let supernetParts, hypernetParts = ip |> splitParts

    supernetParts |> List.exists containsAbba
    && hypernetParts |> List.forall (containsAbba >> not)

let isSslSupported ip =
    let supernetParts, hypernetParts = ip |> splitParts

    let abas = supernetParts |> List.map findAllAbas |> Set.unionMany
    let babs = hypernetParts |> List.map findAllBabs |> Set.unionMany

    Set.intersect abas (babs |> Set.map (fun (b, a) -> a, b))
    |> Set.isEmpty
    |> not

let input = readInputLines "07" |> Seq.toArray

let job1 () =
    input |> Seq.filter isTlsSupported |> Seq.length |> string

let job2 () =
    input |> Seq.filter isSslSupported |> Seq.length |> string
