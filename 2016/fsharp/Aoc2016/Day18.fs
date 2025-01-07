module Day18

open Utils

let next row =
    "." + row + "."
    |> Seq.windowed 3
    |> Seq.map (function
        | [| '^'; '^'; '.' |]
        | [| '.'; '^'; '^' |]
        | [| '^'; '.'; '.' |]
        | [| '.'; '.'; '^' |] -> '^'
        | _ -> '.')
    |> String.ofChars

let countSafeTiles rowCount row =
    let rec loop result rowCount row =
        match rowCount with
        | 0 -> result
        | _ ->
            loop
                (result + Seq.sumBy (fun c -> if c = '.' then 1 else 0) row)
                (rowCount - 1)
                (next row)

    loop 0 rowCount row

let input = readInputText "18"

let job1 () = input |> countSafeTiles 40 |> string

let job2 () = input |> countSafeTiles 400000 |> string
