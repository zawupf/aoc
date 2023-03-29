module Day21

open Utils

let SWAP_POS = @"^swap position (\d+) with position (\d+)$"
let SWAP_LETTER = @"^swap letter (.) with letter (.)$"
let ROT_LEFT = @"^rotate left (\d+) steps?$"
let ROT_RIGHT = @"^rotate right (\d+) steps?$"
let ROT_POS = @"^rotate based on position of letter (.)$"
let REV_POS = @"^reverse positions (\d+) through (\d+)$"
let MOV_POS = @"^move position (\d+) to position (\d+)$"

let rotateLeft x (chars: char[]) =
    let i =
        match x % chars.Length with
        | i when i < 0 -> chars.Length + i
        | i -> i

    Array.concat [ chars.[i..]; chars.[.. i - 1] ]

let rotateRight x (chars: char[]) = rotateLeft (chars.Length - x) chars

let scramble string instructions =
    let rec loop (chars: char[]) instructions =
        match instructions with
        | [] -> chars |> String.ofChars
        | instruction :: instructions ->
            let chars' =
                match instruction with
                | Regex SWAP_POS [ Int x; Int y ] ->
                    let c = chars.[x]
                    chars.[x] <- chars.[y]
                    chars.[y] <- c
                    chars
                | Regex SWAP_LETTER [ Char x; Char y ] ->
                    chars
                    |> Array.map (fun c ->
                        if c = x then y
                        else if c = y then x
                        else c)
                | Regex ROT_LEFT [ Int x ] -> chars |> rotateLeft x
                | Regex ROT_RIGHT [ Int x ] -> chars |> rotateRight x
                | Regex ROT_POS [ Char c ] ->
                    let i = chars |> Array.findIndex ((=) c)
                    let x = 1 + i + (if i >= 4 then 1 else 0)
                    chars |> rotateRight x
                | Regex REV_POS [ Int x; Int y ] ->
                    Array.concat [
                        chars.[.. x - 1]
                        chars.[x..y] |> Array.rev
                        chars.[y + 1 ..]
                    ]
                | Regex MOV_POS [ Int x; Int y ] ->
                    let c = chars.[x]
                    chars |> Array.removeAt x |> Array.insertAt y c
                | _ -> failwith $"Invalid instruction: %s{instruction}"

            loop chars' instructions

    loop (string |> String.toCharArray) instructions

let unscramble string instructions =
    let rec loop (chars: char[]) instructions =
        match instructions with
        | [] -> chars |> String.ofChars
        | instruction :: instructions ->
            let chars' =
                match instruction with
                | Regex SWAP_POS [ Int x; Int y ] ->
                    let c = chars.[x]
                    chars.[x] <- chars.[y]
                    chars.[y] <- c
                    chars
                | Regex SWAP_LETTER [ Char x; Char y ] ->
                    chars
                    |> Array.map (fun c ->
                        if c = x then y
                        else if c = y then x
                        else c)
                | Regex ROT_LEFT [ Int x ] -> chars |> rotateRight x
                | Regex ROT_RIGHT [ Int x ] -> chars |> rotateLeft x
                | Regex ROT_POS [ Char c ] ->
                    let scramble c chars =
                        let i = chars |> Array.findIndex ((=) c)
                        let x = 1 + i + (if i >= 4 then 1 else 0)
                        chars |> rotateRight x
                    // FIXME: brute force
                    { 1 .. chars.Length }
                    |> Seq.map (fun x -> chars |> rotateLeft x)
                    |> Seq.find (fun result -> (result |> scramble c) = chars)
                | Regex REV_POS [ Int x; Int y ] ->
                    Array.concat [
                        chars.[.. x - 1]
                        chars.[x..y] |> Array.rev
                        chars.[y + 1 ..]
                    ]
                | Regex MOV_POS [ Int y; Int x ] ->
                    let c = chars.[x]
                    chars |> Array.removeAt x |> Array.insertAt y c
                | _ -> failwith $"Invalid instruction: %s{instruction}"

            loop chars' instructions

    loop (string |> String.toCharArray) (instructions |> List.rev)

let input = readInputLines "21" |> Array.toList

let job1 () = input |> scramble "abcdefgh"

let job2 () = input |> unscramble "fbgdceah"
