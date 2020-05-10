namespace TicTacToe.Console

open System
open TicTacToe.Domain
open TicTacToe.Console.Shared

type internal Command =
    | Place of Position
    | Undo
    | Restart
    | Quit

[<RequireQualifiedAccess>]
module internal Command =
    let parse (status: GameStatus) (rawCommand: string): Command option =
        let command =
            if rawCommand <> null then rawCommand.ToUpper().Trim()
            else ""
        match command with
        | "U" -> Some Undo
        | "R" -> Some Restart
        | "Q" -> Some Quit
        | _ ->
            match status with
            | OnGoing _ ->
                let (isNumber, i) = Int32.TryParse command
                if isNumber then Position.fromInt i |> Option.map Place
                else None
            | _ -> None
