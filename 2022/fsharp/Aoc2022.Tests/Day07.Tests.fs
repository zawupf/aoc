module Day07.Tests

open Xunit
open Utils
open Day07

let input =
    """
    $ cd /
    $ ls
    dir a
    14848514 b.txt
    8504156 c.dat
    dir d
    $ cd a
    $ ls
    dir e
    29116 f
    2557 g
    62596 h.lst
    $ cd e
    $ ls
    584 i
    $ cd ..
    $ cd ..
    $ cd d
    $ ls
    4060174 j
    8033020 d.log
    5626152 d.ext
    7214296 k
    """
    |> String.trim
    |> String.split '\n'
    |> Array.map String.trim
    |> Array.toList

[<Fact>]
let ``Day07 FS.path works`` () =
    let fs = input |> FS.parseTerminal
    Assert.Equal("/", fs |> Dir |> FS.path)

    Assert.Equal(
        "/a/e",
        fs |> FS.find (fun e -> e |> FS.name = "e") |> List.head |> FS.path
    )

    Assert.Equal(
        "/d/d.log",
        fs |> FS.find (fun e -> e |> FS.name = "d.log") |> List.head |> FS.path
    )

[<Fact>]
let ``Day07 part1 works`` () =
    Assert.Equal(95437UL, input |> FS.parseTerminal |> part1)

[<Fact>]
let ``Day07 Stars`` () =
    try
        Assert.Equal("1432936", job1 ())
        Assert.Equal("272298", job2 ())
    with :? System.NotImplementedException ->
        ()
