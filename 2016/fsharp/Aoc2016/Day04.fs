module Day04

open Utils

type Room = { Name: string; SectorID: int; Checksum: string }

module Room =
    let parse input =
        match input with
        | Regex @"^([a-z\-]+)-(\d+)\[([a-z]+)\]$" [ name; Int sectorId; checksum ] -> {
            Name = name
            SectorID = sectorId
            Checksum = checksum
          }
        | _ -> failwith $"Invalid room: %s{input}"

    let isReal room =
        let { Name = name; Checksum = checksum } = room

        name
        |> String.toCharArray
        |> Seq.filter (fun c -> c <> '-')
        |> Seq.countBy id
        |> Seq.groupBy snd
        |> Seq.sortByDescending fst
        |> Seq.collect (fun (_, chars) ->
            chars |> Seq.map fst |> Seq.sort |> Seq.toArray)
        |> Seq.take 5
        |> Seq.toArray
        |> System.String
        |> string
        |> (=) checksum

    let realName room =
        room.Name
        |> String.toCharArray
        |> Array.map (function
            | '-' -> ' '
            | c ->
                let offset = room.SectorID % 26
                let i = int c - int 'a'
                let j = (i + offset) % 26
                let c' = int 'a' + j
                c' |> char)
        |> System.String
        |> string

let input = readInputLines "04" |> Array.map Room.parse |> Seq.toArray

let job1 () =
    input
    |> Seq.filter Room.isReal
    |> Seq.sumBy (fun room -> room.SectorID)
    |> string

let job2 () =
    input
    |> Seq.filter Room.isReal
    |> Seq.pick (fun room ->
        if room |> Room.realName = "northpole object storage" then
            Some room.SectorID
        else
            None)
    |> string
