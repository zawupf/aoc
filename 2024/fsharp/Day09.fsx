#load "Utils.fsx"

type Block = {
    Id: int
    Used: int
    Free: int
} with

    member this.Length = this.Used + this.Free

type Blocks = ResizeArray<Block>

// let dump (blocks: Blocks) =
//     blocks
//     |> Seq.iter (fun { Id = id; Used = used; Free = free } ->
//         [ 1..used ] |> List.iter (fun _ -> printf "%d" id)
//         [ 1..free ] |> List.iter (fun _ -> printf "."))

//     printfn ""

let parse line =
    line
    |> Seq.chunkBySize 2
    |> Seq.indexed
    |> Seq.map (function
        | id, [| used; free |] -> {
            Id = id
            Used = int used - int '0'
            Free = int free - int '0'
          }
        | id, [| used |] -> {
            Id = id
            Used = int used - int '0'
            Free = 0
          }
        | _ -> failwith "Invalid input")
    |> Seq.toArray
    |> Blocks

let fragment (blocks: Blocks) =
    let findFreeBlockIndex i =
        blocks.FindIndex(i, fun b -> b.Free <> 0)

    let mutable i = findFreeBlockIndex 0
    let mutable j = blocks.Count - 1

    while i < j do
        let bi, bj = blocks[i], blocks[j]

        if bi.Free > bj.Used then
            blocks[i] <- { bi with Free = 0 }
            blocks.RemoveAt j
            i <- i + 1
            blocks.Insert(i, { bj with Free = bi.Free - bj.Used })

            blocks[j] <- {
                blocks[j] with
                    Free = blocks[j].Free + bj.Length
            }
        elif bi.Free < bj.Used then
            blocks[i] <- { bi with Free = 0 }
            i <- i + 1
            blocks.Insert(i, { bj with Used = bi.Free; Free = 0 })
            j <- j + 1

            blocks[j] <- {
                bj with
                    Used = bj.Used - bi.Free
                    Free = bj.Free + bi.Free
            }

            i <- findFreeBlockIndex (i + 1)
        else
            blocks[i] <- { bi with Free = 0 }
            blocks.RemoveAt j
            i <- i + 1
            blocks.Insert(i, { bj with Free = 0 })

            blocks[j] <- {
                blocks[j] with
                    Free = blocks[j].Free + bj.Length
            }

            i <- findFreeBlockIndex (i + 1)

    blocks

let defragment (blocks: Blocks) =
    let findFreeBlockIndex n i j =
        blocks.FindIndex(i, max 0 (j - i), fun b -> b.Free >= n)

    let mutable j = blocks.Count - 1
    let mutable start = findFreeBlockIndex 1 0 j

    while start < j && start <> -1 do
        let bj = blocks[j]

        match findFreeBlockIndex bj.Used start j with
        | -1 -> j <- j - 1
        | i ->
            let bi = blocks[i]

            blocks[i] <- { bi with Free = 0 }
            blocks.RemoveAt j
            blocks.Insert(i + 1, { bj with Free = bi.Free - bj.Used })

            blocks[j] <- {
                blocks[j] with
                    Free = blocks[j].Free + bj.Length
            }

            start <- findFreeBlockIndex 1 start j

    blocks

let checksum (blocks: Blocks) =
    blocks
    |> Seq.fold
        (fun (i, sum) ({ Id = id; Used = used; Free = free }) ->
            let s = [| i .. i + used - 1 |] |> Array.sumBy ((*) id >> int64)
            i + used + free, sum + s)
        (0, 0L)
    |> snd

let part1 input = input |> parse |> fragment |> checksum

let part2 input =
    input |> parse |> defragment |> checksum

let day = __SOURCE_FILE__[3..4]
let input = Utils.readInputText day
let solution1 () = part1 input
let solution2 () = part2 input

let testInput = "2333133121414131402"
Utils.Test.run "Test 1" 1928L (fun () -> part1 testInput)
Utils.Test.run "Test 2" 2858L (fun () -> part2 testInput)

Utils.Test.run "Part 1" 6607511583593L solution1
Utils.Test.run "Part 2" 6636608781232L solution2
