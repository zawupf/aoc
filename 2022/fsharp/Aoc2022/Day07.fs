module Day07

open Utils

type Dir =
    { ParentDir: Dir option
      Name: string
      mutable Entries: Entry list }

and File =
    { ParentDir: Dir
      Name: string
      Size: uint64 }

and Entry =
    | File of File
    | Dir of Dir

module FS =
    let empty () =
        { ParentDir = None
          Name = ""
          Entries = [] }

    let up (dir: Dir) = dir.ParentDir

    let rec root dir =
        match dir |> up with
        | Some d -> root d
        | None -> dir

    let name entry =
        match entry with
        | Dir d -> d.Name
        | File f -> f.Name

    let parentDir entry =
        match entry with
        | Dir d -> d.ParentDir |> Option.map Dir
        | File f -> f.ParentDir |> Dir |> Some

    let path entry =
        let rec loop names entry =
            match entry with
            | None ->
                match names |> String.join "/" with
                | "" -> "/"
                | path -> path
            | Some entry -> loop (name entry :: names) (entry |> parentDir)

        loop [] (Some entry)

    let tryFindEntry entryName dir =
        dir.Entries |> List.tryFind (fun entry -> entry |> name = entryName)

    let cd entry dir =
        match entry with
        | "/" -> dir |> root |> Some
        | ".." -> dir |> up
        | _ ->
            match dir |> tryFindEntry entry with
            | Some(Dir dir) -> Some dir
            | _ -> None

    let rec size entry =
        match entry with
        | File f -> f.Size
        | Dir d -> d.Entries |> List.sumBy size

    let isDir =
        function
        | File _ -> false
        | Dir _ -> true

    let isFile = isDir >> not

    let subentries =
        function
        | File _ -> []
        | Dir { Entries = entries } -> entries

    let find predicate dir =
        let rec loop result entries =
            match entries with
            | [] -> result
            | entry :: entries ->
                let result' =
                    if predicate entry then entry :: result else result

                loop result' (subentries entry @ entries)

        loop [] [ Dir dir ]

    let parseTerminal lines =
        let rec loop pwd lines =
            match lines with
            | [] -> pwd |> root
            | line :: lines ->
                match line with
                | Regex @"^\$ cd (.+)$" [ entry ] ->
                    match pwd |> cd entry with
                    | Some dir -> loop dir lines
                    | None -> failwith $"Invalid dir: %s{entry}"
                | Regex @"^\$ ls$" [] -> loop pwd lines
                | Regex @"^dir (.+)$" [ name ] ->
                    match tryFindEntry name pwd with
                    | Some _ -> ()
                    | None ->
                        pwd.Entries <-
                            ({ ParentDir = Some pwd
                               Name = name
                               Entries = [] }
                             |> Dir)
                            :: pwd.Entries

                    loop pwd lines
                | Regex @"^(\d+) (.+)$" [ UInt64 size; name ] ->
                    match tryFindEntry name pwd with
                    | Some _ -> ()
                    | None ->
                        pwd.Entries <-
                            ({ ParentDir = pwd
                               Name = name
                               Size = size }
                             |> File)
                            :: pwd.Entries

                    loop pwd lines
                | _ -> failwith $"Invalid line: %s{line}"

        loop (empty ()) lines

let part1 fs =
    fs
    |> FS.find FS.isDir
    |> List.map FS.size
    |> List.filter (fun size -> size <= 100000UL)
    |> List.sum

let part2 fs =
    let unusedSpace = 70_000_000UL - (Dir(fs) |> FS.size)
    let freeupSpace = 30_000_000UL - unusedSpace

    fs
    |> FS.find FS.isDir
    |> List.map FS.size
    |> List.filter (fun size -> size >= freeupSpace)
    |> List.min

let input = readInputLines "07" |> Array.toList

let job1 () =
    input |> FS.parseTerminal |> part1 |> string

let job2 () =
    input |> FS.parseTerminal |> part2 |> string
