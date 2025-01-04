#load "Utils.fsx"
open Utils

type Move =
    | Spin of int
    | Exchange of int * int
    | Partner of char * char

let parseMoves moves =
    moves
    |> String.split ","
    |> Array.map (fun move ->
        match move with
        | Regex @"s(\d+)" [ Int n ] -> Spin n
        | Regex @"x(\d+)/(\d+)" [ Int i; Int j ] -> Exchange(i, j)
        | Regex @"p(\w)/(\w)" [ Char a; Char b ] -> Partner(a, b)
        | _ -> failwithf "Invalid move: %s" move)

let swap a b programs =
    let temp = programs |> Array.item a
    programs[a] <- programs[b]
    programs[b] <- temp
    programs

let danceOnce programs moves =
    moves
    |> Array.fold
        (fun programs move ->
            match move with
            | Spin n ->
                programs
                |> Array.splitAt (programs.Length - n)
                |> fun (a, b) -> Array.append b a
            | Exchange(i, j) -> swap i j programs
            | Partner(a, b) ->
                let i = programs |> Array.findIndex (fun x -> x = a)
                let j = programs |> Array.findIndex (fun x -> x = b)
                swap i j programs)
        programs

let dance count initialPrograms moves =
    let programs = initialPrograms |> String.toCharArray
    let moves = moves |> parseMoves

    let history = ResizeArray()

    let rec loop n programs =
        let currentPrograms = programs |> String.ofChars

        if currentPrograms = initialPrograms then
            history[count % n - 1]
        elif n = count then
            currentPrograms
        else
            history.Add currentPrograms
            loop (n + 1) (danceOnce programs moves)

    loop 1 (danceOnce programs moves)

let part1 input = input |> dance 1 "abcdefghijklmnop"

let part2 input =
    input |> dance 1_000_000_000 "abcdefghijklmnop"

let day = __SOURCE_FILE__[3..4]
let input = readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

Test.run "Test 1" "baedc" (fun () -> dance 1 "abcde" "s1,x3/4,pe/b")
Test.run "Test 2" "ceadb" (fun () -> dance 2 "abcde" "s1,x3/4,pe/b")

Test.run "Part 1" "lgpkniodmjacfbeh" solution1
Test.run "Part 2" "hklecbpnjigoafmd" solution2

#load "_benchmark.fsx"
