module Day04

open Utils

let md5 (input: string) =
    use md5 = System.Security.Cryptography.MD5.Create()

    input
    |> System.Text.Encoding.ASCII.GetBytes
    |> md5.ComputeHash
    |> System.Convert.ToHexString

let findLowestNumberWithPrefix (prefix: string) secret =
    let buildMd5 i = i + 1, secret + string (i + 1) |> md5

    let hexWithPrefix (_, md5: string) = md5.StartsWith(prefix)

    buildMd5 |> Seq.initInfinite |> Seq.find hexWithPrefix |> fst

let input = readInputText "04"

let job1 () =
    input |> findLowestNumberWithPrefix "00000" |> string

let job2 () =
    input |> findLowestNumberWithPrefix "000000" |> string
