# Work in progress

Internal cheat written in c# for growtopia

## Requirements
 - .NET 8.0 SDK
 - C++ Compiler with c++ 17 features support

## How to build
  - To build internal open cmd in same dir as the .csproj and write `dotnet publish -c Release -r win-x64 /tl` this will build and compile it to native dll
  - To build injector select Release-x64 and build it

## How to use
  - You need Growtopia 4.19 you can install it from [Here](https://ubistatic-a.akamaihd.net/0098/594764/GrowtopiaInstaller.exe)
  - Then copy the dll file same folder as the builded injector and drag & drop the .dll file to the injector (Native builded .dll can be found in `Sumsum.Internal\bin\Release\net8.0\win-x64\publish` once builded)
  - Currently you cant login because there isn't any login spoof added yet
