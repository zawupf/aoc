module Day19

open Utils

module Ring =
    let init n = [| yield! [ 1 .. n - 1 ]; 0 |]

    let next cursor ring = ring |> Array.item cursor

    let removeAtOffset offset cursor (ring: int[]) =
        assert (offset > 0)

        let rec loop cursor prevCursor offset =
            match offset with
            | 0 ->
                ring.[prevCursor] <- ring.[cursor]
                prevCursor
            | _ -> loop (next cursor ring) cursor (offset - 1)

        loop (next cursor ring) cursor (offset - 1)

let play1 n =
    let ring = Ring.init n

    let rec loop cursor =
        ring |> Ring.removeAtOffset 1 cursor |> ignore

        match ring |> Ring.next cursor with
        | cursor' when cursor' = cursor -> cursor + 1
        | cursor' -> loop cursor'

    loop 0

let play2 n =
    let ring = Ring.init n

    let rec loop cursor offset =
        match ring |> Ring.next cursor with
        | cursor' when cursor' = cursor -> cursor + 1
        | _ -> loop (ring |> Ring.removeAtOffset offset cursor) (3 - offset)

    loop (n / 2 - 1) (if n % 2 = 0 then 2 else 1)

let input = readInputText "19"

let job1 () = input |> int |> play1 |> string

let job2 () = input |> int |> play2 |> string
