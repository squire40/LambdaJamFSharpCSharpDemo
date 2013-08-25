// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Data

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let data = FreebaseData.GetDataContext()

    

    let elements = data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Characters``.Individuals
    //.``Science and Technology``.Chemistry.``Chemical Elements`` |> Seq.toList

    let hydrogen = data.``Science and Technology``.Chemistry.``Chemical Elements``.Individuals.Hydrogen 
    0 // return an integer exit code
