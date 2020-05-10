module internal TicTacToe.Console.Driver

open System
open TicTacToe.Console
open TicTacToe.Console.Effect
open TicTacToe.Console.Reader
open TicTacToe.Domain


let start (): Reader<#IHasConsole, unit> =
    let rec gameLoop error game =
        reader {
            do! Console.clearScreen()

            match error with
            | Some error ->
                do! (Console.write (error + Environment.NewLine + Environment.NewLine))
            | _ -> ()

            let model = Game.model game
            let view = View.game model

            do! Console.write view

            let! rawLine = Console.readLine()
            match Command.parse model.Status rawLine with
            | Some Restart -> do! gameLoop None (Game.start X)
            | Some Undo ->
                match Game.undo game with
                | Ok newGame -> do! gameLoop None newGame
                | Error Game.NoMovesToUndo -> do! gameLoop (Some "There are no moves to undo!") game
            | Some Quit -> return ()
            | Some(Place position) ->
                match Game.play position game with
                | Ok newGame -> do! gameLoop None newGame
                | Error Game.GameIsNotOnGoing ->
                    do! gameLoop (Some "The game is over you cannot place more pieces!") game
                | Error Game.PositionIsNotEmpty ->
                    do! gameLoop (Some "That position is occupied! Please pick another.") game
            | None -> do! gameLoop (Some "Unrecognized command!") game
        }
    gameLoop None (Game.start X)
