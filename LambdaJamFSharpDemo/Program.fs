﻿open FSharp.Data
open FSharp.Data.FreebaseOperators
open System.Linq

[<Literal>]
let apiKey = @"AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg"
type freebaseDataProvider = FreebaseDataProvider<Key=apiKey>
type hero = {Name:string; Gender:string list; Powers:string list}
[<EntryPoint>]
let main argv = 
    let watch = System.Diagnostics.Stopwatch.StartNew()

    let data = freebaseDataProvider.GetDataContext()
//    printf "Getting heroes"
    
    let fictionalChars = data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Universes``

    let marvel = query { 
                        for h in fictionalChars do
                        where (h.Name.Equals "Marvel Universe")
                        }
                        |> Seq.exactlyOne

    let heroes = marvel.Characters
                    |> Seq.filter(fun x -> x.``Powers or Abilities``.Any())
                    |> Seq.toList

    let heroesWithPowers =
        heroes
        |> List.map (fun y -> { Name   = y.Name; 
                                Gender = y.Gender |> Seq.map string |> Seq.toList
                                Powers = y.``Powers or Abilities``|> Seq.map string |> Seq.toList })
    
    let maleHeroes   = heroesWithPowers |> List.filter (fun i -> i.Gender.Contains("Male"))
    let femaleHeroes = heroesWithPowers |> List.filter (fun i -> i.Gender.Contains("Female"))
    let powersFilter x y = y.Powers |> List.exists (fun z -> z = x)
    
    let powers = 
        heroes
        |> Seq.collect (fun y -> y.``Powers or Abilities``)
        |> Seq.distinctBy (fun x -> x.Name)
        |> Seq.sortBy (fun z -> z.Name)
        |> Seq.map (fun x' -> x'.Name)
        |> Seq.toList

    let orderedCounter a b filter = 
        a
        |> List.map (fun x -> x, b |> List.filter (filter x) |> List.length)
        |> List.sortBy (fun x -> snd x)
        |> List.rev

    let topTenPowers heroes' =
        (powers, heroes', powersFilter)
        |||> orderedCounter
        |> Seq.take 10
        |> Seq.toList

    let topTenPowersByCountForMen = maleHeroes |> topTenPowers
    let topTenPowersByCountForWomen = femaleHeroes |> topTenPowers

    watch.Stop()
    System.Console.WriteLine(System.String.Format("Time to get and slice data: {0} minutes, {1} seconds, {2} milliseconds", watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));
    System.Console.ReadKey() |> ignore
    System.Console.Clear()
    System.Console.WriteLine(System.String.Format("Total Super Heroes: {0}", heroes.Count()))
    System.Console.WriteLine()
    System.Console.WriteLine("Top 10 Super Powers by Count")
    System.Console.WriteLine()
    for p in topTenPowers heroesWithPowers do
        printfn "Count: %i \tPower: %s" (snd p) (fst p)
    System.Console.ReadKey() |> ignore
    System.Console.Clear()
    printfn "Top Ten Super Powers for men\r\n"
    for p in topTenPowersByCountForMen do
        printfn "Count: %i \tGender: %s \tPower: %s" 
    //(snd(p), fst (p))
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
