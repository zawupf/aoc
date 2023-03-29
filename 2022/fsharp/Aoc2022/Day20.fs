module Day20

open Utils

[<Measure>]
type nid

let parse (lines: string seq) =
    lines |> Seq.mapi (fun i v -> i * 1<nid>, int64 v) |> Seq.toArray

let indexById nodeId data =
    data |> Array.findIndex (fun (nodeId', _) -> nodeId = nodeId')

let indexByValue v data =
    data |> Array.findIndex (fun (_, value) -> v = value)

let valueAt i data = data |> Array.item (i % data.Length)

let dump nodeId data =
    let i = data |> indexById nodeId
    let left, right = data |> Array.splitAt i
    let data = Array.concat [ right; left ]
    data |> Array.map (snd >> string) |> String.concat ", "

let mix_n n data =
    let len = (data |> Array.length |> int64) - 1L

    let move data nodeId =
        let i = data |> indexById nodeId
        let node = data |> Array.item i
        let v = node |> snd

        // Thanks Annika! 🎉🥳🤗
        // I would have never guessed the correct modulo length myself 🤷‍♂️😊
        let j =
            match ((int64 i) + v) % len |> int with
            | j when j >= 0 -> j
            | j -> (int len) + j

        // data |> Array.removeAt i |> Array.insertAt j node
        if i < j then
            Array.blit data (i + 1) data i (j - i + 1)
            data[j] <- node
        else if i > j then
            Array.blit data j data (j + 1) (i - j)
            data[j] <- node

        data


    [ 0 .. n - 1 ] |> Seq.map ((*) 1<nid>) |> Seq.fold move data

let mix data = mix_n (Array.length data) data

let groveCoordinates key n data =
    let data = data |> Array.map (fun (nodeId, value) -> nodeId, value * key)
    let data = [ 1..n ] |> List.fold (fun data _ -> mix data) data
    let i = data |> indexByValue 0L

    [
        data |> valueAt (i + 1000)
        data |> valueAt (i + 2000)
        data |> valueAt (i + 3000)
    ]
    |> List.map snd

let groveCoordinatesSum key n data =
    data |> groveCoordinates key n |> List.sum

let input = readInputLines "20"

let job1 () =
    input |> parse |> groveCoordinatesSum 1L 1 |> string

let job2 () =
    input |> parse |> groveCoordinatesSum 811589153L 10 |> string
