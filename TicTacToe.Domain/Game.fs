namespace TicTacToe.Domain

type GameStatus =
    | Win of winner: Piece
    | Tie
    | OnGoing of currentTurn: Piece

type GameModel =
    { Board: Board
      Status: GameStatus }

type Game =
    private { Model: GameModel
              History: GameModel list }

[<RequireQualifiedAccess>]
module Game =
    let model (gameState: Game): GameModel =
        gameState.Model

    let start (piece: Piece): Game =
        let model =
            { Board = Board.empty
              Status = OnGoing piece }
        { Model = model
          History = [] }

    let private isFull board =
        let getPiece p = Board.piece p board
        Board.allPositions |> Seq.forall (Option.isSome << getPiece)

    let private isWin board =
        let getPiece p = Board.piece p board
        let lines =
            [| [| (FirstColumn, FirstRow); (SecondColumn, SecondRow); (ThirdColumn, ThirdRow) |]
               [| (ThirdColumn, FirstRow); (SecondColumn, SecondRow); (FirstColumn, ThirdRow) |]
               [| (FirstColumn, FirstRow); (SecondColumn, FirstRow); (ThirdColumn, FirstRow) |]
               [| (FirstColumn, SecondRow); (SecondColumn, SecondRow); (ThirdColumn, SecondRow) |]
               [| (FirstColumn, ThirdRow); (SecondColumn, ThirdRow); (ThirdColumn, ThirdRow) |]
               [| (FirstColumn, FirstRow); (FirstColumn, SecondRow); (FirstColumn, ThirdRow) |]
               [| (SecondColumn, FirstRow); (SecondColumn, SecondRow); (SecondColumn, ThirdRow) |]
               [| (ThirdColumn, FirstRow); (ThirdColumn, SecondRow); (ThirdColumn, ThirdRow) |] |]
        let checkLine line =
            match Seq.map getPiece line |> Seq.distinct |> Seq.toList with
            | [piece] -> piece
            | _ -> None
        
        Seq.tryPick checkLine lines

    let private status lastPlayedPiece currentBoard =
        match isWin currentBoard with
        | Some winner -> Win winner
        | None ->
            if isFull currentBoard then
                Tie
            else
                match lastPlayedPiece with
                | X -> O
                | O -> X
                |> OnGoing

    type PlayError =
        | PositionIsNotEmpty
        | GameIsNotOnGoing

    let play (position: Position) (game: Game): Result<Game, PlayError> =
        let model = game.Model
        match model.Status with
        | OnGoing currentPiece ->
            match Board.piece position model.Board with
            | Some _ -> Error PositionIsNotEmpty
            | None ->
                let newBoard = Board.place currentPiece position model.Board
                let newModel =
                    { Board = newBoard
                      Status = status currentPiece newBoard }
                Ok
                    { Model = newModel
                      History = model :: game.History }
        | _ ->
            Error GameIsNotOnGoing

    type UndoError = | NoMovesToUndo

    let undo (game: Game): Result<Game, UndoError> =
        match game.History with
        | model :: models ->
            Ok
                { Model = model
                  History = models }
        | _ ->
            Error NoMovesToUndo
