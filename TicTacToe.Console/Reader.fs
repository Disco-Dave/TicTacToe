namespace TicTacToe.Console

type internal Reader<'env, 'a> = 'env -> 'a

module internal Reader =
    let wrap (a: 'a): Reader<'env, 'a> = fun _ -> a
    
    let map (f: 'a -> 'b) (ra: Reader<'env, 'a>): Reader<'env, 'b> = fun env ->
        let a = ra env
        f a
        
    let bind (f: 'a -> Reader<'env, 'b>) (ra: Reader<'env, 'a>): Reader<'env, 'b> = fun env ->
        let a = ra env
        (f a) env
        
    type ReaderCe() =
        member __.Bind (ra, f) = bind f ra
        member __.Return a = wrap a
        member __.ReturnFrom ra = ra
        member __.Zero () = wrap ()
        member this.Delay f = f ()
        member __.Combine (ru, ra) = fun env ->
            ru env
            ra env
    
    let reader = ReaderCe()
