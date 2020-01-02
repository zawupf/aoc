open System

let run day fn1 fn2 =
    printfn "\nDay %d" day
    try
        printfn "Result 1: %s" (fn1())
        printfn "Result 2: %s" (fn2())
    with
        | :? NotImplementedException -> ()

[<EntryPoint>]
let main argv =
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
    run 12 Day12.job1 Day12.job2
    0 // return an integer exit code
