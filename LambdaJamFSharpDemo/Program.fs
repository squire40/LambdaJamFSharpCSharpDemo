// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FSharp.Data
open FSharp.Data.FreebaseOperators
open System.Linq

[<Literal>]
let apiKey = @"AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg"
type freebaseDataProvider = FreebaseDataProvider<Key=apiKey>
type hero = {Name:string; Gender:string list; Powers:string list}
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
                    |> List.ofSeq
                    |> List.filter (fun x -> x.``Powers or Abilities``.Any())
                    |> Seq.toList

    let heroesWithPowers =
        heroes
        |> List.map (fun y -> {Name = y.Name; Gender = y.Gender |> Seq.map string |> Seq.toList; Powers = y.``Powers or Abilities``|> Seq.map string |> Seq.toList})
        //|> Seq.toList
        
    let powers = 
        heroes
        |> Seq.collect (fun y -> y.``Powers or Abilities``)
        |> Seq.distinctBy (fun x -> x.Name)
        |> Seq.sortBy (fun z -> z.Name)
        |> Seq.map (fun x' -> x'.Name)
        |> Seq.toList

    let powersByCount = 
        powers
        |> List.map (fun x -> 
                        (x, heroesWithPowers
                            |> List.fold (fun acc y -> 
                                            if y.Powers |> List.exists (fun z -> z = x) then 
                                                acc+1 
                                            else acc) 0))
        |> List.sortBy (fun x -> snd x)
        |> List.rev

    let topTenPowersByCount = 
        powersByCount
        |> Seq.take 10
        |> Seq.toList

    let topTenPowersByCountForMen = 
        powers
        |> List.map (fun x -> 
                        (x, heroesWithPowers |> List.filter (fun i -> i.Gender.Contains("Male"))
                            |> List.fold (fun acc y -> 
                                            if y.Powers |> List.exists (fun z -> z = x) then 
                                                acc+1 
                                            else acc) 0))
        |> List.sortBy (fun x -> snd x)
        |> List.rev
        |> Seq.take 10
        |> Seq.toList

    let topTenPowersByCountForWomen = 
        powers
        |> List.map (fun x -> 
                        (x, heroesWithPowers |> List.filter (fun i -> i.Gender.Contains("Female"))
                            |> List.fold (fun acc y -> 
                                            if y.Powers |> List.exists (fun z -> z = x) then 
                                                acc+1 
                                            else acc) 0))
        |> List.sortBy (fun x -> snd x)
        |> List.rev
        |> Seq.take 10
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
