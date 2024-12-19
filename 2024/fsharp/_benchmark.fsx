#if BENCHMARK

#r "nuget: BenchmarkDotNet"

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<InProcessAttribute true; MemoryDiagnoser>]
type Runner() =
    [<Benchmark>]
    member _.Part1() = solution1 ()

    [<Benchmark>]
    member _.Part2() = solution2 ()

BenchmarkRunner.Run<Runner>() |> ignore

#endif
