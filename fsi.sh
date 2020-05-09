#!/bin/sh

dotnet fsi \
    -r ./TicTacToe.Domain/bin/Debug/netstandard2.0/TicTacToe.Domain.dll \
    -r ./TicTacToe.Console/bin/Debug/netcoreapp3.1/TicTacToe.Console.dll
