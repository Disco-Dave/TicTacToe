open TicTacToe.Console
open TicTacToe.Console.Effect

[<EntryPoint>]
let main _ =
    let eff = AppEffects ()
    Driver.start eff
    0
