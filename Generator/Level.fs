
// NOTE: If warnings appear, you may need to retarget this project to .NET 4.0. Show the Solution
// Pad, right-click on the project node, choose 'Options --> Build --> General' and change the target
// framework to .NET 4.0 or .NET 4.5.

module Generator.World

open System

module Level =

//    type TileTypes = 
//        | Empty = 0
//        | LeftRight = 1
//        | LeftRightBottom = 2
//        | LeftRightTop = 3

//    type Directions = 
//        | Left = 0
//        | Right = 1
//        | Top = 2
//        | Bottom = 3

//    type LevelMap = 
//        {Width: int
//         Height: int
//         Data: TileTypes[]}

    let createHappyPath (width:int) (height:int) =
//        let random = new Random()
//        let randomIndex (startIncluded, endExcluded) = 
//            let range = endExcluded - startIncluded
//            let newIndex = startIncluded + (int (random.NextDouble() * float range))
//            max startIncluded (min newIndex endExcluded)
//
//        let randomDirection = 
//            let index = randomIndex (0, 4)
//            printfn "%A" index
//            enum<Directions>(index)

        let choose trueValue falseValue predicate = if predicate() then trueValue else falseValue
//        let updateAtIndex cellIndex newValue data = Array.mapi (fun i oldValue -> choose newValue oldValue (fun () -> (i = cellIndex))) data
//
//        let startRoom (rooms:TileTypes []) = 
//            let index = randomIndex (0, width)
//            let newRooms = updateAtIndex index TileTypes.LeftRight rooms
//            (index, newRooms)

//        let rec generatePath (lastIndex, rooms) =
//            let randomOffset =
//                let newDirection = 
//                    let index = randomIndex (0, 4)
//                    printfn "%A" index
//                    enum<Directions>(index)
//    
//                match newDirection with
//                    | Directions.Left -> -1
//                    | Directions.Right -> 1
//                    | Directions.Top -> -5
//                    | Directions.Bottom -> 5
//                    | _ -> 0
//            
//            let newIndex = lastIndex + randomOffset
//
//            if newIndex < 25 then
//                if newIndex > 0 then
//                    let newRooms = updateAtIndex newIndex TileTypes.LeftRight rooms
//                    generatePath (newIndex, newRooms)
//                else rooms
//            else rooms
//                         
//        Array.create (width*height) TileTypes.Empty |>
//        startRoom |>
//        generatePath
//
//    let fillEmptySpaces rooms = 
//        rooms
//
//    let makeLevel() = 
//        let width = 5
//        let height = 5
//
//        let data = createHappyPath width height |> fillEmptySpaces
//
//        {Width = width; Height = height; Data = data}



