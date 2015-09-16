namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("blazey.substituter")>]
[<assembly: AssemblyProductAttribute("blazey.substituter")>]
[<assembly: AssemblyDescriptionAttribute("A Castle Windsor facility that substitutes components for unit testing purposes.")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
