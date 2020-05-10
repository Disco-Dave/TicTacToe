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

    let fromInt (i: int): Position option =
        match i with
        | 1 -> Some (FirstColumn, FirstRow)
        | 2 -> Some (SecondColumn, FirstRow)
        | 3 -> Some (ThirdColumn, FirstRow)
        | 4 -> Some (FirstColumn, SecondRow)
        | 5 -> Some (SecondColumn, SecondRow)
        | 6 -> Some (ThirdColumn, SecondRow)
        | 7 -> Some (FirstColumn, ThirdRow)
        | 8 -> Some (SecondColumn, ThirdRow)
        | 9 -> Some (ThirdColumn, ThirdRow)
        | _ -> None
