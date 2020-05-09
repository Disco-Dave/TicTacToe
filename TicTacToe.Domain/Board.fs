namespace TicTacToe.Domain

type Column =
    | FirstColumn
    | SecondColumn
    | ThirdColumn


type Row =
    | FirstRow
    | SecondRow
    | ThirdRow


type Position = Column * Row


type Piece =
    | X
    | O


type private Slot =
    { Position: Position
      Piece: Piece option }


type Board = private | Board of Slot list

[<RequireQualifiedAccess>]
module Board =
    let allPositions: Position seq =
        let columns = [| FirstColumn; SecondColumn; ThirdColumn |]
        let rows = [| FirstRow; SecondRow; ThirdRow |]
        Seq.allPairs columns rows

    let empty: Board =
        let makeSlot position =
            { Position = position
              Piece = None }

        allPositions
        |> Seq.map makeSlot
        |> List.ofSeq
        |> Board


    let place (piece: Piece) (position: Position) (Board board): Board =
        let setPiece ({ Position = p } as slot) =
            if p = position then { slot with Piece = Some piece } else slot

        Board <| List.map setPiece board


    let piece (position: Position) (Board board): Piece option =
        board
        |> Seq.tryFind (fun s -> s.Position = position)
        |> Option.bind (fun s -> s.Piece)
