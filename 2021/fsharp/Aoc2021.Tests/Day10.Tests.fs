module Day10.Tests

open Xunit

let input =
    [ "[({(<(())[]>[[{[]{<()<>>"
      "[(()[<>])]({[<{<<[]>>("
      "{([(<{}[<>[]}>{[]{[(<()>"
      "(((({<>}<{<{<>}{[]{[]{}"
      "[[<[([]))<([[{}[[()]]]"
      "[{[{({}]{}}([{[{{{}}([]"
      "{<[[]]>}<{[{[{[]{()[[[]"
      "[<(<(<(<{}))><([]([]()"
      "<{([([[(<>()){}]>(<<{{"
      "<{([{{}}[<[[[<>{}]]]>[]]" ]

[<Fact>]
let ``Day10 totalSyntaxErrorScore works`` () =
    Assert.Equal(26397, input |> totalSyntaxErrorScore)

[<Fact>]
let ``Day10 middleCompletion works`` () =
    Assert.Equal(288957L, input |> middleCompletion)

[<Fact>]
let ``Day10 Stars`` () =
    Assert.Equal("294195", job1 ())
    Assert.Equal("3490802734", job2 ())
