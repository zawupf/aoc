module Day12

open Utils

let findNumbers input =
    // printfn "%s" input
    let rx = System.Text.RegularExpressions.Regex(@"(\+|-)?(\d+)")
    let matches = rx.Matches(input)
    matches |> Seq.map (fun m -> m.Value |> int)

let removeRed (input: string) =
    let rec findLeftBracket (input: string) index nObject =
        match input.[index] with
        | '}' -> findLeftBracket input (index - 1) (nObject + 1)
        | '{' ->
            match nObject - 1 with
            | 0 -> index
            | nObject -> findLeftBracket input (index - 1) nObject
        | _ -> findLeftBracket input (index - 1) nObject

    let rec findRightBracket (input: string) index nObject =
        match input.[index] with
        | '{' -> findRightBracket input (index + 1) (nObject + 1)
        | '}' ->
            match nObject - 1 with
            | 0 -> index
            | nObject -> findRightBracket input (index + 1) nObject
        | _ -> findRightBracket input (index + 1) nObject

    let rec loop (input: string) (start: int) =
        match input.IndexOf(":\"red\"", start) with
        | -1 -> input
        | index ->
            let i = findLeftBracket input index 1
            let j = findRightBracket input index 1
            loop (input.Substring(0, i + 1) + input.Substring(j)) (i + 2)

    loop input 0

let input = readInputText "12"

let job1 () =
    input |> findNumbers |> Seq.sum |> string

let job2 () =
    input |> removeRed |> findNumbers |> Seq.sum |> string
