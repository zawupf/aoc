module Day02

open Utils

type Box = int * int * int

module Box =
    let parse input =
        match input |> String.parseInts 'x' with
        | [ w; h; l ] -> (w, h, l)
        | _ -> failwithf "Invalid box '%s'" input

    let smallestSides (w, h, l) =
        [| w; h; l |] |> Seq.sort |> Seq.pairwise |> Seq.head

    let surface (w, h, l) = 2 * (w * h + w * l + h * l)

    let volume (w, h, l) = w * h * l

let requiredPaper box =
    let a, b = Box.smallestSides box
    Box.surface box + a * b

let requiredRibbon box =
    let a, b = Box.smallestSides box
    Box.volume box + 2 * (a + b)

let input = readInputLines "02"

let job1 () =
    input |> Seq.sumBy (Box.parse >> requiredPaper) |> string

let job2 () =
    input |> Seq.sumBy (Box.parse >> requiredRibbon) |> string
