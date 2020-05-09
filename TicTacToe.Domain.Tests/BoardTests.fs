module TicTacToe.Domain.Tests.BoardTests

open Xunit
open TicTacToe.Domain

[<Fact>]
let ``Board.allPositions has all 9 spaces`` () =
    let positionCount =
        Board.allPositions
        |> Seq.distinct
        |> Seq.length

    Assert.Equal(9, positionCount)

[<Fact>]
let ``Board.empty initializes a board with all empty slots`` () =
    Board.allPositions
    |> Seq.map (fun p -> Board.piece p Board.empty)
    |> Seq.forall Option.isNone
    |> Assert.True

[<Fact>]
let ``Board.place is able to place piece in all slots that are empty`` () =
    for position in Board.allPositions do
        for piece in [ X; O ] do
            let slot =
                Board.empty
                |> Board.place piece position
                |> Board.piece position

            Assert.Equal(Some piece, slot)

[<Fact>]
let ``Board.place is able to replace piece in all nonempty slots`` () =
    let opposite piece =
        match piece with
        | X -> O
        | O -> X

    for position in Board.allPositions do
        for piece in [ X; O ] do
            let slot =
                Board.empty
                |> Board.place (opposite piece) position
                |> Board.place piece position
                |> Board.piece position

            Assert.Equal(Some piece, slot)
