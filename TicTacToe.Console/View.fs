module internal TicTacToe.Console.View

open System
open TicTacToe.Domain
open TicTacToe.Console.Shared

let private viewSlot board (index, position) =
    match Board.piece position board with
    | None -> sprintf " %d " index
    | Some X -> " X "
    | Some O -> " O "

let private viewRow board positions =
    positions
    |> Seq.map (viewSlot board)
    |> String.concat "|"

let board (board: Board): string =
    Board.allPositions
    |> Seq.map (fun p -> Position.toInt p, p)
    |> Seq.sortBy fst
    |> Seq.chunkBySize 3
    |> Seq.map (viewRow board)
    |> String.concat (Environment.NewLine + "-----------" + Environment.NewLine)
