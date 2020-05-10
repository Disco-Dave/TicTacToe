module TicTacToe.Console.Tests.ViewTests

open Xunit
open TicTacToe.Console
open TicTacToe.Domain


[<Fact>]
let ``View.board can render an empty board``() =
    let view = View.board Board.empty
    let expectedView = " 1 | 2 | 3
-----------
 4 | 5 | 6
-----------
 7 | 8 | 9 "
    Assert.Equal(expectedView, view)


[<Fact>]
let ``View.board can render a non-empty board``() =
    let view =
        Board.empty
        |> Board.place X (SecondColumn, FirstRow)
        |> Board.place O (ThirdColumn, SecondRow)
        |> Board.place X (FirstColumn, SecondRow)
        |> Board.place O (ThirdColumn, ThirdRow)
        |> View.board

    let expectedView = " 1 | X | 3
-----------
 X | 5 | O
-----------
 7 | 8 | O "
    Assert.Equal(expectedView, view)


[<Fact>]
let ``View.board can render a full board``() =
    let view =
        Board.empty
        |> Board.place X (FirstColumn, FirstRow)
        |> Board.place X (SecondColumn, FirstRow)
        |> Board.place O (ThirdColumn, FirstRow)
        |> Board.place X (FirstColumn, SecondRow)
        |> Board.place O (SecondColumn, SecondRow)
        |> Board.place O (ThirdColumn, SecondRow)
        |> Board.place O (FirstColumn, ThirdRow)
        |> Board.place X (SecondColumn, ThirdRow)
        |> Board.place O (ThirdColumn, ThirdRow)
        |> View.board

    let expectedView = " X | X | O
-----------
 X | O | O
-----------
 O | X | O "
    Assert.Equal(expectedView, view)
