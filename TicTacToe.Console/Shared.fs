namespace TicTacToe.Console.Shared

open TicTacToe.Domain

module internal Position =
    let toInt (position: Position): int =
        match position with
        | (FirstColumn, FirstRow) -> 1
        | (SecondColumn, FirstRow) -> 2
        | (ThirdColumn, FirstRow) -> 3
        | (FirstColumn, SecondRow) -> 4
        | (SecondColumn, SecondRow) -> 5
        | (ThirdColumn, SecondRow) -> 6
        | (FirstColumn, ThirdRow) -> 7
        | (SecondColumn, ThirdRow) -> 8
        | (ThirdColumn, ThirdRow) -> 9
