open FSharp.Data
open FSharp.Data.FreebaseOperators
open System.Linq

[<Literal>]
let apiKey = @"AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg"

type freebaseDataProvider = FreebaseDataProvider< Key=apiKey >

type hero = 
    { Name : string
      Gender : string list
      Powers : string list }

type power = 
    { Power : string
      Gender : string
      Count : int }

[<EntryPoint>]
let main argv = 
    let watch = System.Diagnostics.Stopwatch.StartNew()
    let data = freebaseDataProvider.GetDataContext()
    let fictionalChars = 
        data.``Arts and Entertainment``.``Fictional Universes``.``Fictional Universes``
    
    let marvel = 
        query { 
            for h in fictionalChars do
                where (h.Name.Equals "Marvel Universe")
        }
        |> Seq.exactlyOne
    
    let heroes = 
        marvel.Characters
        |> Seq.filter (fun x -> x.``Powers or Abilities``.Any())
        |> Seq.toList
    
    let heroesWithPowers = 
        heroes |> List.map (fun y -> 
                      { Name = y.Name
                        Gender = 
                            y.Gender
                            |> Seq.map string
                            |> Seq.toList
                        Powers = 
                            y.``Powers or Abilities``
                            |> Seq.map string
                            |> Seq.toList })
    
    let maleHeroes = 
        heroesWithPowers |> List.filter (fun i -> i.Gender.Contains("Male"))
    let femaleHeroes = 
        heroesWithPowers |> List.filter (fun i -> i.Gender.Contains("Female"))
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
        |> List.map (fun x -> 
               x, 
               b
               |> List.filter (filter x)
               |> List.length)
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
    System.Console.WriteLine
        (System.String.Format
             ("Time to get and slice data: {0} minutes, {1} seconds, {2} milliseconds", 
              watch.Elapsed.Minutes, watch.Elapsed.Seconds, 
              watch.Elapsed.Milliseconds))
    System.Console.ReadKey() |> ignore
    System.Console.Clear()
    System.Console.WriteLine
        (System.String.Format("Total Super Heroes: {0}", heroes.Count()))
    System.Console.WriteLine()
    System.Console.WriteLine("Top 10 Super Powers by Count")
    System.Console.WriteLine()
    for p in topTenPowers heroesWithPowers do
        printfn "Count: %i \tPower: %s" (snd p) (fst p)
    System.Console.ReadKey() |> ignore
    System.Console.Clear()
    printfn "Top Ten Super Powers for men\r\n"
    System.Console.ReadKey() |> ignore
    for p in topTenPowersByCountForMen do
        printfn "Count: %i \tPower: %s" (snd p) (fst p)
    System.Console.ReadKey() |> ignore
    System.Console.Clear()
    printfn "Top Ten Super Powers for women\r\n"
    System.Console.ReadKey() |> ignore
    for p in topTenPowersByCountForWomen do
        printfn "Count: %i \tPower: %s" (snd p) (fst p)
    System.Console.ReadKey() |> ignore
    System.Console.Clear()
    printfn "Female heroes having the top 5 powers:\r\n\r\n"
    let printHerosHavingPowerList (powersList : (string * int) list) 
        (heroesList : hero list) = 
        for p in powersList |> Seq.take 5 do
            let power' = fst p
            let hlist = 
                heroesList |> List.filter (fun x -> x.Powers.Contains power')
            for i in 0..hlist.Length do
                printfn "%A" (hlist.Item(i).Name)
                if i % 15 = 0 then 
                    printfn "\r\nPress any key...\r\n"
                    System.Console.ReadKey() |> ignore
                    System.Console.Clear()
    //        |> ignore
    //    printfn ""
    printHerosHavingPowerList topTenPowersByCountForWomen femaleHeroes
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
      
