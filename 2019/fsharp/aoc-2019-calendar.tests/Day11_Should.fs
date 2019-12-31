module Tests11

open Xunit
open Day11

[<Fact>]
let ``Day11 Stars``() =
    Assert.Equal("2276", job1())
    Assert.Equal
        (System.String.Join
            ("",
             [| "\n  ##  ###  #    ###    ## ####  ##  #  #   "
                "\n #  # #  # #    #  #    #    # #  # #  #   "
                "\n #    ###  #    #  #    #   #  #    #  #   "
                "\n #    #  # #    ###     #  #   #    #  #   "
                "\n #  # #  # #    #    #  # #    #  # #  #   "
                "\n  ##  ###  #### #     ##  ####  ##   ##    " |]), job2())
    ()
