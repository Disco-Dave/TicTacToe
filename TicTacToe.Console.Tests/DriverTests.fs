module TicTacToe.Console.Tests.DriverTests

open Xunit
open System.Text
open TicTacToe.Console
open TicTacToe.Console.Effect


type private MockConsole(readLines: string list) =
    let output = StringBuilder()
    let mutable readLines = readLines

    interface IConsole with
        member __.ClearScreen() = output.AppendLine("--- CLEAR SCREEN ---") |> ignore

        member __.Write message = output.Append message |> ignore

        member __.ReadLine() =
            match readLines with
            | line :: lines ->
                readLines <- lines
                line
            | _ ->
                ""

    member __.GetOutput() = output.ToString()
    

type private MockEffects(console: IConsole) =
    interface IHasConsole with member __.Console = console
    
    
let private simulate readLines =
    let console = MockConsole (readLines @ ["q"]) // Ensures that we terminate the game loop.
    let mockEffects = MockEffects console
    Driver.start () mockEffects
    console.GetOutput ()
    

[<Fact>]
let ``Game starts with empty board and X as the first player`` () =
    let output = simulate []
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)

[<Fact>]
let ``Game informs the user about unrecognized commands`` () =
    let output = simulate ["!"]
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
Unrecognized command!

 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)

[<Fact>]
let ``Game informs the user when there are no moves to undo`` () =
    let output = simulate ["u"]
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
There are no moves to undo!

 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)
    
[<Fact>]
let ``Game informs the user when a space is occupied`` () =
    let output = simulate ["1"; "1"]
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
That position is occupied! Please pick another.

 X | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)
    
    
[<Fact>]
let ``The game informs the user when the game is over and does not let them place any more pieces`` () =
    let output = simulate ["1"; "2"; "4"; "3"; "7"; "5"]
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | 3 
-----------
 X | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | O 
-----------
 X | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | O 
-----------
 X | 5 | 6 
-----------
 X | 8 | 9 

X won! [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
Unrecognized command!

 X | O | O 
-----------
 X | 5 | 6 
-----------
 X | 8 | 9 

X won! [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)
    
[<Fact>]
let ``The game can undo moves`` () =
    let output = simulate ["1"; "u";]
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)
    
[<Fact>]
let ``The game may be restarted`` () =
    let output = simulate ["1"; "2"; "4"; "3"; "7"; "r"]
    let expectedOutput = "--- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | 3 
-----------
 X | 5 | 6 
-----------
 7 | 8 | 9 

O's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | O 
-----------
 X | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 X | O | O 
-----------
 X | 5 | 6 
-----------
 X | 8 | 9 

X won! [u]ndo, [r]estart, or [q]uit: --- CLEAR SCREEN ---
 1 | 2 | 3 
-----------
 4 | 5 | 6 
-----------
 7 | 8 | 9 

X's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: "
    Assert.Equal(expectedOutput, output)
