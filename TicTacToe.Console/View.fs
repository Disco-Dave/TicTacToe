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

let commandPrompt (status: GameStatus) =
    match status with
    | OnGoing piece ->
        sprintf "%s's turn. [1..9] to place a piece, [u]ndo, [r]estart, or [q]uit: " (piece.ToString())
    | Tie ->
        "Tie game! [u]ndo, [r]estart, or [q]uit: "
    | Win piece ->
        sprintf "%s won! [u]ndo, [r]estart, or [q]uit: " (piece.ToString())

let game (model: GameModel): string =
    board model.Board + Environment.NewLine + Environment.NewLine + commandPrompt model.Status
