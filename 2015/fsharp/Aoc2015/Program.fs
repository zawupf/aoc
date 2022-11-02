open System

let tryJob job =
    try
        let stopWatch = Diagnostics.Stopwatch.StartNew()
        let value = job ()
        stopWatch.Stop()
        let time = stopWatch.Elapsed.TotalMilliseconds
        Some(value, time)
    with :? NotImplementedException ->
        None

let run day job1 job2 =
    let result1 = tryJob job1
    let result2 = tryJob job2

    match result1, result2 with
    | Some _, _
    | _, Some _ -> printfn "\nDay %d" day
    | _ -> ()

    match result1 with
    | Some(value, time) -> printfn "Result 1: %s (%.3fms)" value time
    | _ -> ()

    match result2 with
    | Some(value, time) -> printfn "Result 2: %s (%.3fms)" value time
    | _ -> ()

let runDay =
    [| (fun () -> run 1 Day01.job1 Day01.job2)
       (fun () -> run 2 Day02.job1 Day02.job2)
       (fun () -> run 3 Day03.job1 Day03.job2)
       (fun () -> run 4 Day04.job1 Day04.job2)
       (fun () -> run 5 Day05.job1 Day05.job2)
       (fun () -> run 6 Day06.job1 Day06.job2)
       (fun () -> run 7 Day07.job1 Day07.job2)
       (fun () -> run 8 Day08.job1 Day08.job2)
       (fun () -> run 9 Day09.job1 Day09.job2)
       (fun () -> run 10 Day10.job1 Day10.job2)
       (fun () -> run 11 Day11.job1 Day11.job2)
       (fun () -> run 12 Day12.job1 Day12.job2)
       (fun () -> run 13 Day13.job1 Day13.job2)
       (fun () -> run 14 Day14.job1 Day14.job2)
       (fun () -> run 15 Day15.job1 Day15.job2)
       (fun () -> run 16 Day16.job1 Day16.job2)
       (fun () -> run 17 Day17.job1 Day17.job2)
       (fun () -> run 18 Day18.job1 Day18.job2)
       (fun () -> run 19 Day19.job1 Day19.job2)
       (fun () -> run 20 Day20.job1 Day20.job2)
       (fun () -> run 21 Day21.job1 Day21.job2)
       (fun () -> run 22 Day22.job1 Day22.job2)
       (fun () -> run 23 Day22.job1 Day23.job2)
       (fun () -> run 24 Day22.job1 Day24.job2)
       (fun () -> run 25 Day22.job1 Day25.job2) |]

module Bench =
    open BenchmarkDotNet.Attributes
    open BenchmarkDotNet.Running
    open BenchmarkDotNet.Jobs

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_01_1() =
        [<Benchmark>]
        member _.day_01_1() = Day01.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_01_2() =
        [<Benchmark>]
        member _.day_01_2() = Day01.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_02_1() =
        [<Benchmark>]
        member _.day_02_1() = Day02.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_02_2() =
        [<Benchmark>]
        member _.day_02_2() = Day02.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_03_1() =
        [<Benchmark>]
        member _.day_03_1() = Day03.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_03_2() =
        [<Benchmark>]
        member _.day_03_2() = Day03.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_04_1() =
        [<Benchmark>]
        member _.day_04_1() = Day04.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_04_2() =
        [<Benchmark>]
        member _.day_04_2() = Day04.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_05_1() =
        [<Benchmark>]
        member _.day_05_1() = Day05.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_05_2() =
        [<Benchmark>]
        member _.day_05_2() = Day05.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_06_1() =
        [<Benchmark>]
        member _.day_06_1() = Day06.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_06_2() =
        [<Benchmark>]
        member _.day_06_2() = Day06.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_07_1() =
        [<Benchmark>]
        member _.day_07_1() = Day07.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_07_2() =
        [<Benchmark>]
        member _.day_07_2() = Day07.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_08_1() =
        [<Benchmark>]
        member _.day_08_1() = Day08.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_08_2() =
        [<Benchmark>]
        member _.day_08_2() = Day08.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_09_1() =
        [<Benchmark>]
        member _.day_09_1() = Day09.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_09_2() =
        [<Benchmark>]
        member _.day_09_2() = Day09.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_10_1() =
        [<Benchmark>]
        member _.day_10_1() = Day10.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_10_2() =
        [<Benchmark>]
        member _.day_10_2() = Day10.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_11_1() =
        [<Benchmark>]
        member _.day_11_1() = Day11.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_11_2() =
        [<Benchmark>]
        member _.day_11_2() = Day11.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_12_1() =
        [<Benchmark>]
        member _.day_12_1() = Day12.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_12_2() =
        [<Benchmark>]
        member _.day_12_2() = Day12.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_13_1() =
        [<Benchmark>]
        member _.day_13_1() = Day13.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_13_2() =
        [<Benchmark>]
        member _.day_13_2() = Day13.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_14_1() =
        [<Benchmark>]
        member _.day_14_1() = Day14.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_14_2() =
        [<Benchmark>]
        member _.day_14_2() = Day14.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_15_1() =
        [<Benchmark>]
        member _.day_15_1() = Day15.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_15_2() =
        [<Benchmark>]
        member _.day_15_2() = Day15.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_16_1() =
        [<Benchmark>]
        member _.day_16_1() = Day16.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_16_2() =
        [<Benchmark>]
        member _.day_16_2() = Day16.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_17_1() =
        [<Benchmark>]
        member _.day_17_1() = Day17.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_17_2() =
        [<Benchmark>]
        member _.day_17_2() = Day17.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_18_1() =
        [<Benchmark>]
        member _.day_18_1() = Day18.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_18_2() =
        [<Benchmark>]
        member _.day_18_2() = Day18.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_19_1() =
        [<Benchmark>]
        member _.day_19_1() = Day19.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_19_2() =
        [<Benchmark>]
        member _.day_19_2() = Day19.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_20_1() =
        [<Benchmark>]
        member _.day_20_1() = Day20.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_20_2() =
        [<Benchmark>]
        member _.day_20_2() = Day20.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_21_1() =
        [<Benchmark>]
        member _.day_21_1() = Day21.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_21_2() =
        [<Benchmark>]
        member _.day_21_2() = Day21.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_22_1() =
        [<Benchmark>]
        member _.day_22_1() = Day22.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_22_2() =
        [<Benchmark>]
        member _.day_22_2() = Day22.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_23_1() =
        [<Benchmark>]
        member _.day_23_1() = Day23.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_23_2() =
        [<Benchmark>]
        member _.day_23_2() = Day23.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_24_1() =
        [<Benchmark>]
        member _.day_24_1() = Day24.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_24_2() =
        [<Benchmark>]
        member _.day_24_2() = Day24.job2 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_25_1() =
        [<Benchmark>]
        member _.day_25_1() = Day25.job1 ()

    [<SimpleJob(RuntimeMoniker.HostProcess)>]
    type Benchmark_25_2() =
        [<Benchmark>]
        member _.day_25_2() = Day25.job2 ()

    let bench day part =
        match day, part with
        | "01", "1" -> BenchmarkRunner.Run<Benchmark_01_1>() |> ignore
        | "01", "2" -> BenchmarkRunner.Run<Benchmark_01_2>() |> ignore
        | "02", "1" -> BenchmarkRunner.Run<Benchmark_02_1>() |> ignore
        | "02", "2" -> BenchmarkRunner.Run<Benchmark_02_2>() |> ignore
        | "03", "1" -> BenchmarkRunner.Run<Benchmark_03_1>() |> ignore
        | "03", "2" -> BenchmarkRunner.Run<Benchmark_03_2>() |> ignore
        | "04", "1" -> BenchmarkRunner.Run<Benchmark_04_1>() |> ignore
        | "04", "2" -> BenchmarkRunner.Run<Benchmark_04_2>() |> ignore
        | "05", "1" -> BenchmarkRunner.Run<Benchmark_05_1>() |> ignore
        | "05", "2" -> BenchmarkRunner.Run<Benchmark_05_2>() |> ignore
        | "06", "1" -> BenchmarkRunner.Run<Benchmark_06_1>() |> ignore
        | "06", "2" -> BenchmarkRunner.Run<Benchmark_06_2>() |> ignore
        | "07", "1" -> BenchmarkRunner.Run<Benchmark_07_1>() |> ignore
        | "07", "2" -> BenchmarkRunner.Run<Benchmark_07_2>() |> ignore
        | "08", "1" -> BenchmarkRunner.Run<Benchmark_08_1>() |> ignore
        | "08", "2" -> BenchmarkRunner.Run<Benchmark_08_2>() |> ignore
        | "09", "1" -> BenchmarkRunner.Run<Benchmark_09_1>() |> ignore
        | "09", "2" -> BenchmarkRunner.Run<Benchmark_09_2>() |> ignore
        | "10", "1" -> BenchmarkRunner.Run<Benchmark_10_1>() |> ignore
        | "10", "2" -> BenchmarkRunner.Run<Benchmark_10_2>() |> ignore
        | "11", "1" -> BenchmarkRunner.Run<Benchmark_11_1>() |> ignore
        | "11", "2" -> BenchmarkRunner.Run<Benchmark_11_2>() |> ignore
        | "12", "1" -> BenchmarkRunner.Run<Benchmark_12_1>() |> ignore
        | "12", "2" -> BenchmarkRunner.Run<Benchmark_12_2>() |> ignore
        | "13", "1" -> BenchmarkRunner.Run<Benchmark_13_1>() |> ignore
        | "13", "2" -> BenchmarkRunner.Run<Benchmark_13_2>() |> ignore
        | "14", "1" -> BenchmarkRunner.Run<Benchmark_14_1>() |> ignore
        | "14", "2" -> BenchmarkRunner.Run<Benchmark_14_2>() |> ignore
        | "15", "1" -> BenchmarkRunner.Run<Benchmark_15_1>() |> ignore
        | "15", "2" -> BenchmarkRunner.Run<Benchmark_15_2>() |> ignore
        | "16", "1" -> BenchmarkRunner.Run<Benchmark_16_1>() |> ignore
        | "16", "2" -> BenchmarkRunner.Run<Benchmark_16_2>() |> ignore
        | "17", "1" -> BenchmarkRunner.Run<Benchmark_17_1>() |> ignore
        | "17", "2" -> BenchmarkRunner.Run<Benchmark_17_2>() |> ignore
        | "18", "1" -> BenchmarkRunner.Run<Benchmark_18_1>() |> ignore
        | "18", "2" -> BenchmarkRunner.Run<Benchmark_18_2>() |> ignore
        | "19", "1" -> BenchmarkRunner.Run<Benchmark_19_1>() |> ignore
        | "19", "2" -> BenchmarkRunner.Run<Benchmark_19_2>() |> ignore
        | "20", "1" -> BenchmarkRunner.Run<Benchmark_20_1>() |> ignore
        | "20", "2" -> BenchmarkRunner.Run<Benchmark_20_2>() |> ignore
        | "21", "1" -> BenchmarkRunner.Run<Benchmark_21_1>() |> ignore
        | "21", "2" -> BenchmarkRunner.Run<Benchmark_21_2>() |> ignore
        | "22", "1" -> BenchmarkRunner.Run<Benchmark_22_1>() |> ignore
        | "22", "2" -> BenchmarkRunner.Run<Benchmark_22_2>() |> ignore
        | "23", "1" -> BenchmarkRunner.Run<Benchmark_23_1>() |> ignore
        | "23", "2" -> BenchmarkRunner.Run<Benchmark_23_2>() |> ignore
        | "24", "1" -> BenchmarkRunner.Run<Benchmark_24_1>() |> ignore
        | "24", "2" -> BenchmarkRunner.Run<Benchmark_24_2>() |> ignore
        | "25", "1" -> BenchmarkRunner.Run<Benchmark_25_1>() |> ignore
        | "25", "2" -> BenchmarkRunner.Run<Benchmark_25_2>() |> ignore
        | _ -> failwith "Unknown benchmark"

[<EntryPoint>]
let main argv =
    match argv with
    | [||] ->
        run 1 Day01.job1 Day01.job2
        run 2 Day02.job1 Day02.job2
        run 3 Day03.job1 Day03.job2
        run 4 Day04.job1 Day04.job2
        run 5 Day05.job1 Day05.job2
        run 6 Day06.job1 Day06.job2
        run 7 Day07.job1 Day07.job2
        run 8 Day08.job1 Day08.job2
        run 9 Day09.job1 Day09.job2
        run 10 Day10.job1 Day10.job2
        run 11 Day11.job1 Day11.job2
        run 12 Day12.job1 Day12.job2
        run 13 Day13.job1 Day13.job2
        run 14 Day14.job1 Day14.job2
        run 15 Day15.job1 Day15.job2
        run 16 Day16.job1 Day16.job2
        run 17 Day17.job1 Day17.job2
        run 18 Day18.job1 Day18.job2
        run 19 Day19.job1 Day19.job2
        run 20 Day20.job1 Day20.job2
        run 21 Day21.job1 Day21.job2
        run 22 Day22.job1 Day22.job2
        run 23 Day22.job1 Day23.job2
        run 24 Day22.job1 Day24.job2
        run 25 Day22.job1 Day25.job2
    | [| "-b"; day; part |] -> Bench.bench day part
    | _ -> argv |> Seq.iter (fun arg -> runDay.[(arg |> int) - 1] ())

    0
