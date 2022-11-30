module Tests03

open Xunit
open Day03

[<Theory>]
[<InlineData(159,
             "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83")>]
[<InlineData(135,
             "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7")>]
let ``Day03 minDistance works`` expected (input: string) =
    Assert.Equal(expected, minDistance (input.Split '\n'))
    ()

[<Theory>]
[<InlineData(610,
             "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83")>]
[<InlineData(410,
             "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7")>]
let ``Day03 minSignalDelay works`` expected (input: string) =
    Assert.Equal(expected, minSignalDelay (input.Split '\n'))
    ()

[<Fact>]
let ``Day03 Stars`` () =
    Assert.Equal("8015", job1 ())
    Assert.Equal("163676", job2 ())
