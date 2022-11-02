module Day08

open Utils

let decodeCharCount (literal: string) =
    if literal.StartsWith('"') |> not || literal.EndsWith('"') |> not then
        failwithf "Invalid literal: %s" literal
    else
        let mutable n = 0
        let iend = literal.Length - 1
        let mutable i = 1

        while i < iend do
            n <- n + 1

            i <-
                i
                + match literal.[i] with
                  | '\\' ->
                      if i + 1 = iend then
                          failwith "Invalid escape sequence at end"
                      else
                          match literal.[i + 1] with
                          | '\\'
                          | '"' -> 2
                          | 'x' -> 4
                          | c -> failwithf "Invalid escape sequence \\%c" c
                  | _ -> 1

        n

let decodeOverhead literal =
    (literal |> String.length) - (literal |> decodeCharCount)

let encodeCharCount (string: string) =
    string
    |> Seq.fold (fun len c -> len + if c = '"' || c = '\\' then 2 else 1) 2

let encodeOverhead literal =
    (literal |> encodeCharCount) - (literal |> String.length)

let input = readInputLines "08"

let job1 () =
    input |> Seq.sumBy decodeOverhead |> string

let job2 () =
    input |> Seq.sumBy encodeOverhead |> string
