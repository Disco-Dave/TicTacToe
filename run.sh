#!/bin/sh

if [ ! -f ./TicTacToe.Console/bin/Release/netcoreapp3.1/TicTacToe.Console ]; then
    dotnet build --configuration Release
fi

./TicTacToe.Console/bin/Release/netcoreapp3.1/TicTacToe.Console
