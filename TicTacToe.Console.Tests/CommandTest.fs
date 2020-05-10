module TicTacToe.Console.Tests.CommandTest

open Xunit
open TicTacToe.Domain
open TicTacToe.Console.Shared
open TicTacToe.Console

[<Fact>]
let ``Able to parse position when on going`` () =
    for position in Board.allPositions do
        let rawCommand =
            position
            |> Position.toInt
            |> string
            
        Assert.Equal (Some (Place position), Command.parse (OnGoing X) rawCommand)
        
[<Fact>] 
let ``Unable to parse position when not on going`` () =
    for status in [Tie; Win X;] do
        for position in Board.allPositions do
            let rawCommand =
                position
                |> Position.toInt
                |> string
                
            Assert.Equal (None, Command.parse status rawCommand)
            
[<Fact>]
let ``Able to parse undo`` () =
    for status in [OnGoing O;Tie; Win X;] do
        for rawCommand in [" U "; "  u   "; "u"; "U"; "\tu"] do
            Assert.Equal (Some Undo, Command.parse status rawCommand)
            
[<Fact>]
let ``Able to parse restart`` () =
    for status in [OnGoing O;Tie; Win X;] do
        for rawCommand in [" R "; "  r   "; "r"; "R"; "\tr"] do
            Assert.Equal (Some Restart, Command.parse status rawCommand)
            
[<Fact>]            
let ``Able to parse quit`` () =
    for status in [OnGoing O;Tie; Win X;] do
        for rawCommand in [" Q "; "  q   "; "q"; "Q"; "\tq"] do
            Assert.Equal (Some Quit, Command.parse status rawCommand)
