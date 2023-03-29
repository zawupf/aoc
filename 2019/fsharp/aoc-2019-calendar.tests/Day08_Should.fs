module Tests08

open System
open Xunit
open Day08

[<Fact>]
let ``Day08 Stars`` () =
    Assert.Equal("2806", job1 ())

    Assert.Equal(
        Utils.String.join "" [|
            "\n**** ***    **  **  ***  "
            "\n   * *  *    * *  * *  * "
            "\n  *  ***     * *  * ***  "
            "\n *   *  *    * **** *  * "
            "\n*    *  * *  * *  * *  * "
            "\n**** ***   **  *  * ***  "
        |],
        job2 ()
    )

    ()
