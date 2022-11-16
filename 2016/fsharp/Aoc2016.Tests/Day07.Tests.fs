module Day07.Tests

open Xunit
open Utils
open Day07

[<Theory>]
[<InlineData(true, "abba[mnop]qrst")>]
[<InlineData(false, "abcd[bddb]xyyx")>]
[<InlineData(false, "aaaa[qwer]tyui")>]
[<InlineData(true, "ioxxoj[asdfgh]zxcvbn")>]
let ``Day07 isTlsSupported works`` expected ip =
    Assert.Equal(expected, ip |> isTlsSupported)

[<Theory>]
[<InlineData(true, "aba[bab]xyz")>]
[<InlineData(false, "xyx[xyx]xyx")>]
[<InlineData(true, "aaa[kek]eke")>]
[<InlineData(true, "zazbz[bzb]cdb")>]
let ``Day07 isSslSupported works`` expected ip =
    Assert.Equal(expected, ip |> isSslSupported)

[<Fact>]
let ``Day07 Stars`` () =
    try
        Assert.Equal("110", job1 ())
        Assert.Equal("242", job2 ())
    with :? System.NotImplementedException ->
        ()
