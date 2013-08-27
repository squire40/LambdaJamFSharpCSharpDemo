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
    printfn "Getting heroes"
//    let getHeroes = 
//        query { for h in data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Characters`` do
//                where (Array.toList h.``Appears In These Fictional Universes`` = "Wolverine")
//                select h 
//                }
//        |> Seq.toList
        
    let presidents = 
        query { for e in data.Society.Government.``US Presidents`` do 
                select e.Name } 
        |> Seq.toList

    printfn "%A" presidents
    let blah = presidents
    let input = System.Console.ReadKey
    0 // return an integer exit code
