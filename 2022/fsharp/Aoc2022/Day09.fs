module Day09

open Utils

type Pos = int * int
type Rope = Pos list

module Rope =
    let init length = List.replicate length (0, 0)

    let head = List.head

    let tail = List.last

    let step dir rope =
        let moveHead (hx, hy) =
            match dir with
            | 'U' -> hx, hy + 1
            | 'D' -> hx, hy - 1
            | 'R' -> hx + 1, hy
            | 'L' -> hx - 1, hy
            | _ -> failwith $"Invalid direction: %c{dir}"

        let rec moveTails moved tails =
            match tails with
            | [] -> moved |> List.rev
            | (tx, ty) :: tails ->
                let hx, hy = moved |> List.head
                let dx, dy = hx - tx, hy - ty

                let dtx, dty =
                    if abs dx > 1 || abs dy > 1 then sign dx, sign dy else 0, 0

                let tail = tx + dtx, ty + dty
                moveTails (tail :: moved) tails

        moveTails [ rope |> head |> moveHead ] (rope |> List.tail)

    let move steps rope =
        seq {
            yield rope

            let mutable r = rope

            for step' in steps do
                match step' with
                | Regex @"^(.) (\d+)$" [ Char dir; Int count ] ->
                    for _ in 1..count do
                        r <- r |> step dir
                        yield r
                | _ -> failwith $"Invalid step: %s{step'}"
        }

let countTailPositions n steps =
    let rope = Rope.init n

    rope
    |> Rope.move steps
    |> Seq.fold
        (fun tailPositions rope -> tailPositions |> Set.add (rope |> Rope.tail))
        Set.empty
    |> Set.count


let input = readInputLines "09"

let job1 () = input |> countTailPositions 2 |> string

let job2 () =
    input |> countTailPositions 10 |> string
