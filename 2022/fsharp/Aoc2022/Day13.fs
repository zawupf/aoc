module Day13

open Utils

type CompareResult =
    | Valid
    | Invalid
    | Undetermined

type Data =
    | Number of int
    | Packet of Data list

module Data =
    let parse line =
        let rec loop stack i =
            match String.length line = i with
            | true ->
                match stack with
                | [ data ] -> data
                | _ -> failwith "Error on parse end"
            | false ->
                match line[i] with
                | '[' -> loop (Packet([]) :: stack) (i + 1)
                | ']' ->
                    match stack with
                    | Packet(current) :: Packet(data) :: stack ->
                        loop
                            (Packet(Packet(current |> List.rev) :: data)
                             :: stack)
                            (i + 1)
                    | [ Packet(current) ] ->
                        loop [ Packet(current |> List.rev) ] (i + 1)
                    | _ -> failwith "Error on packet close"
                | ',' -> loop stack (i + 1)
                | c when c >= '0' && c <= '9' ->
                    match line.IndexOfAny([| ','; ']' |], i) with
                    | j when j > i ->
                        let n = line[i .. j - 1] |> int

                        match stack with
                        | Packet(data) :: stack ->
                            loop (Packet(Number(n) :: data) :: stack) j
                        | _ -> failwith "Error on adding number"
                    | _ -> failwith "Error on parsing number"
                | c -> failwith $"Invalid char: %c{c}"

        loop [] 0

    let parsePairs (input: string) =
        input.Split("\n\n")
        |> Array.map (
            String.split '\n'
            >> Array.map parse
            >> function
                | [| d1; d2 |] -> d1, d2
                | _ -> failwith "Not a data pair"
        )

    let dump data =
        let d = sprintf "%A" data
        d.Replace("Packet ", "").Replace("Number ", "").Replace("; ", ",")

    let rec compare d1 d2 =
        match d1, d2 with
        | Number d1, Number d2 ->
            if d1 < d2 then Valid
            else if d1 > d2 then Invalid
            else Undetermined
        | Number n, Packet _ -> compare (Packet [ Number n ]) d2
        | Packet _, Number n -> compare d1 (Packet [ Number n ])
        | Packet d1, Packet d2 -> loop d1 d2

    and loop d1 d2 =
        match d1, d2 with
        | a :: d1, b :: d2 ->
            match compare a b with
            | Undetermined -> loop d1 d2
            | result -> result
        | [], _ :: _ -> Valid
        | _ :: _, [] -> Invalid
        | [], [] -> Undetermined

    let sort data =
        let rec insert p packets result =
            match packets with
            | [] -> (p :: result) |> List.rev
            | p' :: packets ->
                match compare p p' with
                | Valid -> (result |> List.rev) @ (p :: p' :: packets)
                | Invalid -> insert p packets (p' :: result)
                | Undetermined -> failwith "Compare must not be Undetermined"

        let rec loop result packets =
            match packets with
            | [] -> result |> List.toArray
            | p :: packets -> loop (insert p result []) packets

        loop [ data |> Array.head ] (data |> Array.tail |> Array.toList)

    let sort2 =
        Array.sortWith (fun a b ->
            compare a b
            |> function
                | Valid -> -1
                | Invalid -> 1
                | Undetermined -> 0)

let part1 pairs =
    pairs
    |> Array.mapi (fun i (d1, d2) ->
        match Data.compare d1 d2 with
        | Valid -> i + 1
        | _ -> 0)
    |> Array.sum

let part2 sort pairs =
    let p1 = Packet [ Packet [ Number 2 ] ]
    let p2 = Packet [ Packet [ Number 6 ] ]

    pairs
    |> Array.append [| p1, p2 |]
    |> Array.map (fun (d1, d2) -> [| d1; d2 |])
    |> Array.concat
    |> sort
    |> Array.mapi (fun i p ->
        match p = p1 || p = p2 with
        | true -> i + 1
        | false -> 1)
    |> Array.reduce (*)

let input = readInputText "13"

let job1 () =
    input |> Data.parsePairs |> part1 |> string

let job2 () =
    input |> Data.parsePairs |> part2 Data.sort2 |> string
