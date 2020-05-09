module TicTacToe.Domain.Tests.GameTests

open System
open Xunit
open TicTacToe.Domain


let private curry f a b = f (a, b)

let private simulate piece positions =
    let game = Game.start piece
    match positions with
    | [] -> Ok game
    | [ p ] -> Game.play p game
    | p :: ps ->
        List.fold (fun g p' -> Result.bind (Game.play p') g) (Game.play p game) ps

[<Fact>]
let ``Game.start has an empty board and is ongoing`` () =
    for piece in [ X; O ] do
        let model = Game.model (Game.start piece)
        Assert.Equal(Board.empty, model.Board)
        Assert.Equal(OnGoing piece, model.Status)

[<Fact>]
let ``Game.play allows you to play a piece anywhere that is empty and flips the current piece`` () =
    for piece in [ X; O ] do
        for position in Board.allPositions do
            let otherPiece =
                match piece with
                | O -> X
                | X -> O
            let model =
                Game.start piece
                |> Game.play position
                |> Result.map Game.model

            let status = Result.map (fun m -> m.Status) model
            let slot = Result.map (fun m -> Board.piece position m.Board) model
            Assert.Equal(Ok(OnGoing otherPiece), status)
            Assert.Equal(Ok(Some piece), slot)

[<Fact>]
let ``Game.play errors with PositionIsNotEmpty when trying to place a piece in a position that is occupied`` () =
    for piece in [ X; O ] do
        for position in Board.allPositions do
            let game =
                Game.start piece
                |> Game.play position
                |> Result.bind (Game.play position)

            Assert.Equal(Error Game.PositionIsNotEmpty, game)

[<Fact>]
let ``Game.play identifies a tie`` () =
    for piece in [ X; O ] do
        let status =
            [ (FirstColumn, FirstRow)
              (SecondColumn, FirstRow)
              (ThirdColumn, FirstRow)
              (SecondColumn, SecondRow)
              (ThirdColumn, SecondRow)
              (FirstColumn, SecondRow)
              (SecondColumn, ThirdRow)
              (ThirdColumn, ThirdRow)
              (FirstColumn, ThirdRow) ]
            |> simulate piece
            |> Result.map (Game.model >> fun m -> m.Status)

        Assert.Equal(Ok Tie, status)


[<Fact>]
let ``Game.play identifies a win`` () =
    for piece in [ X; O ] do
        let simulateWin positions =
            positions
            |> simulate piece
            |> Result.map (Game.model >> fun m -> m.Status)
            |> curry Assert.Equal (Ok <| Win piece)

        [ (FirstColumn, FirstRow)
          (FirstColumn, SecondRow)
          (SecondColumn, FirstRow)
          (SecondColumn, SecondRow)
          (ThirdColumn, FirstRow) ]
        |> simulateWin

        [ (FirstColumn, SecondRow)
          (FirstColumn, FirstRow)
          (SecondColumn, SecondRow)
          (ThirdColumn, ThirdRow)
          (ThirdColumn, SecondRow) ]
        |> simulateWin

        [ (FirstColumn, ThirdRow)
          (SecondColumn, SecondRow)
          (ThirdColumn, ThirdRow)
          (SecondColumn, FirstRow)
          (SecondColumn, ThirdRow) ]
        |> simulateWin

        [ (FirstColumn, FirstRow)
          (SecondColumn, FirstRow)
          (FirstColumn, SecondRow)
          (SecondColumn, ThirdRow)
          (FirstColumn, ThirdRow) ]
        |> simulateWin

        [ (SecondColumn, SecondRow)
          (FirstColumn, SecondRow)
          (SecondColumn, FirstRow)
          (ThirdColumn, FirstRow)
          (SecondColumn, ThirdRow) ]
        |> simulateWin

        [ (ThirdColumn, SecondRow)
          (FirstColumn, FirstRow)
          (ThirdColumn, ThirdRow)
          (FirstColumn, ThirdRow)
          (ThirdColumn, FirstRow) ]
        |> simulateWin

        [ (SecondColumn, SecondRow)
          (FirstColumn, SecondRow)
          (FirstColumn, FirstRow)
          (ThirdColumn, FirstRow)
          (ThirdColumn, ThirdRow) ]
        |> simulateWin

        [ (FirstColumn, ThirdRow)
          (FirstColumn, SecondRow)
          (ThirdColumn, FirstRow)
          (FirstColumn, FirstRow)
          (SecondColumn, SecondRow) ]
        |> simulateWin


[<Fact>]
let ``Game.undo returns error when there are no moves to undo.`` () =
    let game = Game.start X |> Game.undo
    Assert.Equal(Error Game.NoMovesToUndo, game)


[<Fact>]
let ``Game.undo undoes moves in order.`` () =
    let mutable game =
        [ (FirstColumn, ThirdRow)
          (FirstColumn, SecondRow)
          (ThirdColumn, FirstRow)
          (FirstColumn, FirstRow)
          (SecondColumn, SecondRow) ]
        |> simulate X
        |> function
        | Error _ -> failwith "Shouldn't have been an error."
        | Ok g -> g

    let states =
        [ [ (FirstColumn, ThirdRow)
            (FirstColumn, SecondRow)
            (ThirdColumn, FirstRow)
            (FirstColumn, FirstRow) ]

          [ (FirstColumn, ThirdRow)
            (FirstColumn, SecondRow)
            (ThirdColumn, FirstRow) ]

          [ (FirstColumn, ThirdRow)
            (FirstColumn, SecondRow) ]

          [ (FirstColumn, ThirdRow) ]

          [] ]

    for state in states do
        game <-
            Game.undo game
            |> function
            | Error _ -> failwith "Shouldn't have been an error."
            | Ok g -> g

        let expectedGame =
            state
            |> simulate X
            |> function
            | Error _ -> failwith "Shouldn't have been an error."
            | Ok g -> g

        Assert.Equal(Ok (Game.model expectedGame).Board, Ok (Game.model game).Board)
        Assert.Equal(Ok (Game.model expectedGame).Status, Ok (Game.model game).Status)
