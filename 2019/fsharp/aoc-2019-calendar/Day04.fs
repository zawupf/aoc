module Day04

let isValidAdjacentDigits number =
    number.ToString().ToCharArray()
    |> Array.windowed 2
    |> Array.exists (fun a -> a.[0] = a.[1])

let isValidAdjacentDigits2 number =
    number.ToString().ToCharArray()
    |> Array.fold (fun (list, char) c ->
        if c = char then (list.Head + 1 :: list.Tail, c)
        else (1 :: list, c)) ([], ' ')
    |> fst
    |> List.exists (fun n -> n = 2)

let isValidNeverDecrease number =
    number.ToString().ToCharArray()
    |> Array.windowed 2
    |> Array.forall (fun a -> a.[0] <= a.[1])

let job1() =
    seq { 128392 .. 643281 }
    |> Seq.filter (fun i -> isValidAdjacentDigits i && isValidNeverDecrease i)
    |> Seq.length
    |> string

let job2() =
    seq { 128392 .. 643281 }
    |> Seq.filter (fun i -> isValidAdjacentDigits2 i && isValidNeverDecrease i)
    |> Seq.length
    |> string
