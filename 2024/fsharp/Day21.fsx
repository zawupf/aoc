#load "Utils.fsx"

type KeyMap = Utils.Dictionary<char * char, string list>

let keyMap =
    let reverse dirs =
        dirs
        |> String.map (function
            | '^' -> 'v'
            | 'v' -> '^'
            | '<' -> '>'
            | '>' -> '<'
            | c -> failwithf "Invalid direction: %c" c)
        |> Seq.rev
        |> Utils.String.ofChars

    let (|KeyPair|_|) ab =
        match ab |> Utils.String.toCharArray with
        | [| a; b |] -> Some(a, b)
        | _ -> None

    let createKeyMap pathInfo : KeyMap =
        List.concat [
            [ for c in "^v<>A0123456789" -> (c, c), [ "A" ] ]
            pathInfo
            |> List.collect (fun row ->
                row
                |> Utils.String.split " "
                |> Array.toList
                |> List.collect (fun info ->
                    match info |> Utils.String.split ":" |> Array.toList with
                    | KeyPair(a, b) :: dirs -> [
                        (a, b), dirs |> List.map (fun dir -> dir + "A")
                        (b, a),
                        dirs |> List.map (fun dir -> (dir |> reverse) + "A")
                      ]
                    | _ -> failwith "Invalid empty path info"))
        ]
        |> Utils.Dictionary.ofSeq

    createKeyMap [
        // Code Keypad Layout:
        // 7 8 9
        // 4 5 6
        // 1 2 3
        //   0 A
        "A3:^ A6:^^ A9:^^^ A0:< A2:<^:^< A5:<^^:^^< A8:<^^^:^^^< A1:^<< A4:^^<< A7:^^^<<"
        "02:^ 03:^>:>^ 01:^< 05:^^ 06:^^>:>^^ 04:^^< 08:^^^ 09:^^^>:>^^^ 07:^^^<"
        "36:^ 39:^^ 32:< 35:^<:<^ 38:^^<:<^^ 31:<< 34:^<<:<<^ 37:^^<<:<<^^"
        "21:< 26:^>:>^ 25:^ 24:^<:<^ 28:^^ 29:^^>:>^^ 27:^^<:<^^"
        "14:^ 15:^>:>^ 16:^>>:>>^ 17:^^ 18:^^>:>^^ 19:^^>>:>>^^"
        "65:< 64:<< 69:^ 68:<^:^< 67:^<<:<<^"
        "54:< 59:^>:>^ 58:^ 57:^<:<^"
        "47:^ 48:^>:>^ 49:^>>:>>^"
        "98:< 97:<<"
        "87:<"

        // Direction Keypad Layout:
        //   ^ A
        // < v >
        "A^:< A>:v Av:v<:<v A<:v<<"
        "^v:v ^>:v>:>v ^<:v<"
        ">v:< ><:<<"
        "v<:<"
    ]

let countKeys n keys =
    let getPairCountPerLevel = Utils.useCache<int * (char * char), int64> ()

    let rec loop n keys =
        if n = 0 then
            keys |> String.length |> int64
        else
            Seq.concat [ Seq.singleton 'A'; seq keys ]
            |> Seq.pairwise
            |> Seq.sumBy (fun pair ->
                getPairCountPerLevel (n, pair)
                <| fun () ->
                    keyMap
                    |> Utils.Dictionary.get pair
                    |> List.map (fun keys -> loop (n - 1) keys)
                    |> List.min)


    loop n keys

let codeValue (code: string) = code[.. code.Length - 2] |> int64

let part1 input =
    input |> Array.sumBy (fun code -> countKeys 3 code * codeValue code)

let part2 input =
    input |> Array.sumBy (fun code -> countKeys 26 code * codeValue code)

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputLines day
let solution1 () = part1 input
let solution2 () = part2 input

Utils.Test.run "Part 1" 215374L solution1
Utils.Test.run "Part 2" 260586897262600L solution2

#load "_benchmark.fsx"
