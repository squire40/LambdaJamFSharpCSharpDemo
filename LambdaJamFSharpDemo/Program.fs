// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Data

[<Literal>]
let apiKey = "AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg"
type freebaseDataProvider = FreebaseDataProvider<Key=apiKey>

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    let data = freebaseDataProvider.GetDataContext() 
    data.DataContext.Limit <- 10
    let elements = data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Characters`` |> Seq.toList

//    let rec getData = 
        
    printfn "%A" elements
    let hydrogen = data.``Science and Technology``.Chemistry.``Chemical Elements``.Individuals.Hydrogen 
    0 // return an integer exit code
