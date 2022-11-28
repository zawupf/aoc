module Day17

open Utils

let hash salt steps = Md5.ofString (salt + steps)

let move direction (x, y) =
    match direction with
    | 'U' -> (x, y - 1)
    | 'D' -> (x, y + 1)
    | 'L' -> (x - 1, y)
    | 'R' -> (x + 1, y)
    | _ -> failwith "Invalid direction"
    |> function
        | x, y when x >= 0 && x < 4 && y >= 0 && y < 4 -> Some(x, y)
        | _ -> None

let nextSteps salt path =
    let pos, steps = path

    hash salt steps
    |> Seq.take 4
    |> Seq.zip "UDLR"
    |> Seq.choose (fun (step, hash) ->
        if hash < 'B' then
            None
        else
            move step pos |> Option.map (fun pos -> pos, steps + string step))

let walk salt =
    let rec loop result ways =
        let max = fst result

        match ways with
        | [] -> result |> snd |> snd
        | (length, _) :: ways when length >= max -> loop result ways
        | (length, (pos, steps)) :: ways when pos = (3, 3) ->
            loop (length, (pos, steps)) ways
        | (length, path) :: ways ->
            let ways' =
                nextSteps salt path
                |> Seq.fold (fun ways path -> (length + 1, path) :: ways) ways

            loop result ways'

    loop (System.Int32.MaxValue, ((0, 0), "")) [ 0, ((0, 0), "") ]

let explore salt =

    let rec loop result ways =
        match ways with
        | [] -> result
        | (length, (pos, _)) :: ways when pos = (3, 3) ->
            loop (max result length) ways
        | (length, path) :: ways ->
            let ways' =
                nextSteps salt path
                |> Seq.fold (fun ways path -> (length + 1, path) :: ways) ways

            loop result ways'

    loop 0 [ 0, ((0, 0), "") ]

let input = readInputText "17"

let job1 () = input |> walk

let job2 () = input |> explore |> string
