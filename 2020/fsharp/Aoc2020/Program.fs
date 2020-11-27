open System

let run day fn1 fn2 =
    printfn "\nDay %d" day
    try
        printfn "Result 1: %s" (fn1 ())
        printfn "Result 2: %s" (fn2 ())
    with :? NotImplementedException -> ()

[<EntryPoint>]
let main argv =
    run 0 Day00.job1 Day00.job2 // Testing
    // run 1 Day01.job1 Day01.job2
    // run 2 Day02.job1 Day02.job2
    // run 3 Day03.job1 Day03.job2
    // run 4 Day04.job1 Day04.job2
    // run 5 Day05.job1 Day05.job2
    // run 6 Day06.job1 Day06.job2
    // run 7 Day07.job1 Day07.job2
    // run 8 Day08.job1 Day08.job2
    // run 9 Day09.job1 Day09.job2
    // run 10 Day10.job1 Day10.job2
    // run 11 Day11.job1 Day11.job2
    // run 12 Day12.job1 Day12.job2
    // run 13 Day13.job1 Day13.job2
    // run 14 Day14.job1 Day14.job2
    // run 15 Day15.job1 Day15.job2
    // run 16 Day16.job1 Day16.job2
    // run 17 Day17.job1 Day17.job2
    // run 18 Day18.job1 Day18.job2
    // run 19 Day19.job1 Day19.job2
    // run 20 Day20.job1 Day20.job2
    // run 21 Day21.job1 Day21.job2
    // run 22 Day22.job1 Day22.job2
    // run 23 Day22.job1 Day23.job2
    // run 24 Day22.job1 Day24.job2
    // run 25 Day22.job1 Day25.job2
    0 // return an integer exit code
