module Day25

open Utils

type Snafu = string

module Snafu =
    let toInt input =
        Seq.foldBack
            (fun c (sum, prod) ->
                let n =
                    match c with
                    | '0' -> 0L
                    | '1' -> 1L
                    | '2' -> 2L
                    | '-' -> -1L
                    | '=' -> -2L
                    | _ -> failwith "Invalid input"

                sum + prod * n, prod * 5L)
            input
            (0L, 1L)
        |> fst

    let ofInt n =
        n
        |> Seq.unfold (fun n ->
            match n / 5L, n % 5L with
            | 0L, 0L -> None
            | n, d when d < 3L -> Some(char (int64 '0' + d), n)
            | n, 3L -> Some('=', n + 1L)
            | n, 4L -> Some('-', n + 1L)
            | _ -> failwith "Internal error")
        |> Seq.rev
        |> String.ofChars
        |> function
            | "" -> "0"
            | s -> s

let input = readInputLines "25"

let job1 () =
    input |> Seq.sumBy Snafu.toInt |> Snafu.ofInt

let job2 () =
    // input |> String.join "" |> string
    raise (System.NotImplementedException())
