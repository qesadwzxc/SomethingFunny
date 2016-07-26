// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

//[<EntryPoint>]
//let main argv = 
//    printfn "%A" argv
//    0 // return an integer exit code
#light
//let main argv=

//let results = [ for i in 0 .. 100 -> (i, i*i) ]
////%d，%f，%s分别是int、float、string的占位符。%A则可用于打印任何值。
//printfn "results = %A" results

let addNum a=a*a
let c= addNum 5
let d = List.map (fun x -> x % 2 = 0) [1 .. 10];;
let f = false::d//数组添加元素
let concat (x:string) y=x+y
let s=concat "Hello" "world"
printfn "Concat:%s %d" s c
printfn "results = %A" f

open System
Console.ReadKey(true)