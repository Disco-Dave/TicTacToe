module internal TicTacToe.Console.Effect

open System

[<Interface>]
type internal IConsole =
    abstract member ReadLine : unit -> string
    abstract member Write: string -> unit
    abstract member ClearScreen : unit -> unit
    
type private Console() =
    interface IConsole with
        member __.ReadLine () = Console.ReadLine ()
        member __.Write message = Console.Write message
        member __.ClearScreen () = Console.Clear ()

[<Interface>]
type internal IHasConsole =
    abstract member Console : IConsole
    
module internal Console =
    let readLine () (eff: #IHasConsole) = eff.Console.ReadLine ()
    let write message (eff: #IHasConsole) = eff.Console.Write message
    let clearScreen () (eff: #IHasConsole) = eff.Console.ClearScreen ()
    
[<Struct>]
type internal AppEffects =
    interface IHasConsole with
        member __.Console = Console() :> IConsole
