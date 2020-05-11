module internal TicTacToe.Console.Driver

open System
open TicTacToe.Console
open TicTacToe.Console.Effect
open TicTacToe.Domain


let start env =
    let clearScreen = Console.clearScreen env
    let readLine = Console.readLine env
    let write = Console.write env

    let rec gameLoop error game =
        clearScreen ()

        match error with
        | Some error ->
            write (error + Environment.NewLine + Environment.NewLine)
        | _ -> ()

        let model = Game.model game
        let view = View.game model

        write view

        let rawLine = readLine ()
        match Command.parse model.Status rawLine with
        | Some Restart -> gameLoop None (Game.start X)
        | Some Undo ->
            match Game.undo game with
            | Ok newGame -> gameLoop None newGame
            | Error Game.NoMovesToUndo -> gameLoop (Some "There are no moves to undo!") game
        | Some Quit -> ()
        | Some (Place position) ->
            match Game.play position game with
            | Ok newGame -> gameLoop None newGame
            | Error Game.GameIsNotOnGoing ->
                gameLoop (Some "The game is over you cannot place more pieces!") game
            | Error Game.PositionIsNotEmpty ->
                gameLoop (Some "That position is occupied! Please pick another.") game
        | None -> gameLoop (Some "Unrecognized command!") game

    gameLoop None (Game.start X)
