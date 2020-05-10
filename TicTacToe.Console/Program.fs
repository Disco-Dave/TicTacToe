open TicTacToe.Console
open TicTacToe.Console.Effect

[<EntryPoint>]
let main _ =
    Driver.start () (AppEffects())
    0
