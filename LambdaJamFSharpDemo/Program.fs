// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Data
open FSharp.Data.FreebaseOperators
open System.Linq

[<Literal>]
let apiKey = @"AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg"
type freebaseDataProvider = FreebaseDataProvider<Key=apiKey>
type hero = {Name:string; Gender:string; Powers:string}
[<EntryPoint>]
let main argv = 
    let data = freebaseDataProvider.GetDataContext()
    printf "Getting heroes"
    
    let fictionalChars = data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Universes``

    let marvel = query { 
                        for h in fictionalChars do
                        where (h.Name.Equals "Marvel Universe")
                        }
                        |> Seq.exactlyOne

    let heroes = marvel.Characters
                    |> Seq.filter (fun x -> x.``Powers or Abilities``.Count() > 0)
                    |> Seq.toList

    let heroesWithPowers =
        heroes
        |> Seq.map (fun y -> {Name = y.Name; Gender = System.String.Join(", ", y.Gender); Powers = System.String.Join(", ", y.``Powers or Abilities``)})
        |> Seq.toList
        
    let powers = 
        heroes
        |> Seq.collect (fun y -> y.``Powers or Abilities``)
        |> Seq.distinctBy (fun a -> a.Name)
        |> Seq.sortBy (fun z -> z.Name)
        |> Seq.toList

    let powersByCount = 
        query { for p in powers do
                for h in heroes do
                where (System.String.Join(", ", h.``Powers or Abilities``).Contains(p.Name)) 
                groupBy p into grp
                sortByDescending (grp.Count())
                select (grp.Key, grp.Count()) }
                |> Seq.toList

    let name = "Dave"        

//    for h in heroes do
//        printfn "%A" h
//    let heroList = heroes |> Seq.toList
//    let heroesWithPowers = query { for h in heroes do
//                                    where (not (h.``Powers or Abilities``.Equals"")) }
//                                    |> Seq.toList


    
//    let powers = query { for p in heroes do
//                            select p.``Powers or Abilities`` }
////                            |> Seq.toList

//    let doOutput = (fun s -> Seq.iter |> System.Console.WriteLine s)
//    doOutput powers
//    for p in powers do
//        System.Console.WriteLine "%A" p
//        printfn "%A" p

    let input = System.Console.ReadKey
    0 // return an integer exit code
