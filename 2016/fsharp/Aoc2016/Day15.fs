module Day15

open Utils

type Disc = Disc of pos: int * count: int

module Disc =
    let modulo a b =
        match a % b with
        | m when m >= 0 -> m
        | m -> b + m

    let withPos pos (Disc(_, count)) = Disc(modulo pos count, count)

    let tick seconds (Disc(pos, count)) =
        Disc(modulo (pos + seconds) count, count)

    let parse line =
        let pattern =
            @"Disc #(?:\d+) has (\d+) positions; at time=0, it is at position (\d+)."

        match line with
        | Regex pattern [ Int count; Int pos ] -> Disc(pos, count)
        | _ -> failwith $"Invalid disc: %s{line}"

let findDropTime discs =
    let target =
        discs |> Array.mapi (fun i disc -> disc |> Disc.withPos -(i + 1))

    let rec loop index discs =
        if discs = target then
            index
        else
            loop (index + 1) (discs |> Array.map (Disc.tick 1))

    loop 0 discs

let input = readInputLines "15"

let job1 () = input |> Array.map Disc.parse |> findDropTime |> string

let job2 () =
    [| yield! (input |> Array.map Disc.parse); Disc(0, 11) |]
    |> findDropTime
    |> string
