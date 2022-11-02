module Day13

open Utils

let parse input =
    let parseLine line =
        let pattern =
            @"^(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+)\.$"

        match line with
        | Regex pattern [ person1; sympathy; happiness; person2 ] ->
            let h =
                match sympathy with
                | "gain" -> happiness |> int
                | "lose" -> (happiness |> int) * -1
                | _ -> failwithf "Invalid input: %s" line

            (person1, person2), h
        | _ -> failwithf "Invalid input: %s" line

    input
    |> Seq.fold
        (fun map line ->
            let persons, happiness = line |> parseLine
            map |> Map.add persons happiness)
        Map.empty<string * string, int>

let persons map =
    map |> Map.keys |> Seq.map (fun (person, _) -> person) |> Seq.distinct

let happiness persons map = map |> Map.find persons

let totalHappiness map seats =
    let person = seats |> Seq.head

    Seq.append seats (Seq.singleton person)
    |> Seq.pairwise
    |> Seq.sumBy (fun persons ->
        let p1, p2 = persons
        (happiness persons map) + (happiness (p2, p1) map))

let maxTotalHappiness map =
    let persons = map |> persons |> Seq.toList

    let first, others =
        match persons with
        | first :: others -> first, others
        | _ -> failwith "No guests?"

    let happiness seats = (first :: seats) |> totalHappiness map

    others |> Math.permutations |> Seq.maxBy happiness |> happiness

let insertMe map =
    map
    |> persons
    |> Seq.fold
        (fun map person ->
            map |> Map.add ("Me", person) 0 |> Map.add (person, "Me") 0)
        map

let input = readInputLines "13"

let job1 () =
    input |> parse |> maxTotalHappiness |> string

let job2 () =
    input |> parse |> insertMe |> maxTotalHappiness |> string
