module Day08

open System
open Utils

type Layer = string

type Image = Layer seq

let private parseImage width height string = string |> Seq.chunkBySize (width * height)

let private pixelStats layer =
    layer
    |> Seq.countBy id
    |> Map.ofSeq

let private checksum image =
    image
    |> Seq.map (fun layer ->
        let ps = layer |> pixelStats

        let count char =
            ps
            |> Map.tryFind char
            |> Option.defaultValue 0

        let count0 = count '0'
        let count1 = count '1'
        let count2 = count '2'
        count0, count1 * count2)
    |> Seq.minBy fst
    |> snd

let private compose a b =
    Array.zip a b
    |> Array.map (function
        | '2', color -> color
        | color, _ -> color)

let private paint line =
    line
    |> Array.map (function
        | '0' -> ' '
        | '1' -> '*'
        | _ -> '.')

let render width image =
    let join (lines: string seq) = Utils.String.join "" lines
    image
    |> Seq.reduce compose
    |> paint
    |> Seq.chunkBySize width
    |> Seq.map
        (String
         >> sprintf "\n%s")
    |> join


let job1() =
    readInputText "08"
    |> parseImage 25 6
    |> checksum
    |> string

let job2() =
    readInputText "08"
    |> parseImage 25 6
    |> render 25
