module Day06

open Utils

let corrected chooser lines =
    lines
    |> Array.map String.toCharArray
    |> Array.transpose
    |> Array.map (fun chars -> chars |> Seq.countBy id |> chooser snd |> fst)
    |> String.ofChars

let input = readInputLines "06" |> Seq.toArray

let job1 () = input |> corrected Seq.maxBy

let job2 () = input |> corrected Seq.minBy
