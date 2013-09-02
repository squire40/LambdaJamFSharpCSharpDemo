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
    data.DataContext.Limit <- 100
    printfn "Getting heroes"
    let heroes = query { for h in data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Characters`` do
                            where ((h.``Appears In These Fictional Universes``.Equals "Marvel") && (not(h.``Powers or Abilities``.Equals "")))
                            select (h.Name, h.Gender, h.``Powers or Abilities``) }
                            |> Seq.toList

//    let heroList = heroes |> Seq.toList
//    let heroesWithPowers = query { for h in heroes do
//                                    where (not (h.``Powers or Abilities``.Equals"")) }
//                                    |> Seq.toList

//    let powers = query { for p in heroesWithPowers do
//                            select p.``Powers or Abilities`` }
//                            |> Seq.toList

//    let doOutput = (fun s -> Seq.iter |> System.Console.WriteLine s)
//    doOutput powers
//    for p in powers do
//        System.Console.WriteLine "%A" p
//        printfn "%A" p

    let input = System.Console.ReadKey
    0 // return an integer exit code
