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


[<Fact>]
let ``View.commandPrompt show correct commands for OnGoing game`` () =
    for piece in [X; O] do
        let status = OnGoing piece
        let expectedPrompt =
            sprintf "%s's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: " (piece.ToString())
        Assert.Equal (expectedPrompt, View.commandPrompt status)
        
[<Fact>]
let ``View.commandPrompt show correct commands for Tie game`` () =
    let status = Tie
    let expectedPrompt = "Tie game! [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal (expectedPrompt, View.commandPrompt status)
    
[<Fact>]
let ``View.commandPrompt show correct commands for Won game`` () =
    for piece in [X; O] do
        let status = Win piece
        let expectedPrompt =
            sprintf "%s won! [u]ndo, [r]estart, or [q]uit: " (piece.ToString())
        Assert.Equal (expectedPrompt, View.commandPrompt status)
        
[<Fact>]
let ``View.game can render new game`` () =
    for piece in [X; O] do
        let model =
            { Board = Board.empty
              Status = OnGoing piece }
        let expectedView = sprintf " 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

%s's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: " (piece.ToString())
        Assert.Equal (expectedView, View.game model)
    
    
[<Fact>]
let ``View.game can render non-emtpy on going game`` () =
    for piece in [X; O] do
        let board =
            Board.empty
            |> Board.place X (SecondColumn, FirstRow)
            |> Board.place O (ThirdColumn, SecondRow)
            |> Board.place X (FirstColumn, SecondRow)
            |> Board.place O (ThirdColumn, ThirdRow)
        let model =
            { Board = board
              Status = OnGoing piece }
        let expectedView = sprintf " 1 | X | 3 
-----------
 X | 5 | O 
-----------
 7 | 8 | O 

%s's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: " (piece.ToString())
        Assert.Equal (expectedView, View.game model)
        
[<Fact>]
let ``View.game can render tie game`` () =
    let board =
        Board.empty
        |> Board.place X (FirstColumn, FirstRow)
        |> Board.place O (SecondColumn, FirstRow)
        |> Board.place X (ThirdColumn, FirstRow)
        |> Board.place X (FirstColumn, SecondRow)
        |> Board.place O (SecondColumn, SecondRow)
        |> Board.place O (ThirdColumn, SecondRow)
        |> Board.place O (FirstColumn, ThirdRow)
        |> Board.place X (SecondColumn, ThirdRow)
        |> Board.place O (ThirdColumn, ThirdRow)
    let model =
        { Board = board
          Status = Tie }
    let expectedView = " X | O | X 
-----------
 X | O | O 
-----------
 O | X | O 

Tie game! [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal (expectedView, View.game model)
    
[<Fact>]
let ``View.game can render won game`` () =
    let board =
        Board.empty
        |> Board.place X (FirstColumn, FirstRow)
        |> Board.place X (SecondColumn, FirstRow)
        |> Board.place X (ThirdColumn, FirstRow)
        |> Board.place X (FirstColumn, SecondRow)
        |> Board.place O (SecondColumn, SecondRow)
        |> Board.place O (ThirdColumn, SecondRow)
        |> Board.place O (FirstColumn, ThirdRow)
        |> Board.place X (SecondColumn, ThirdRow)
        |> Board.place O (ThirdColumn, ThirdRow)
    let model =
        { Board = board
          Status = Win X }
    let expectedView = " X | X | X 
-----------
 X | O | O 
-----------
 O | X | O 

X won! [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal (expectedView, View.game model)
