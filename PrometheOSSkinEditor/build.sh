#!/bin/bash
dotnet restore
dotnet publish --no-restore -p:Platform=x64 -r win-x64 ./PrometheOSSkinEditor.csproj -c Release --self-contained true -p:DebugType=None -p:DebugSymbols=false
dotnet publish --no-restore -p:Platform=x64 -r linux-x64 ./PrometheOSSkinEditor.csproj -c Release --self-contained true -p:DebugType=None -p:DebugSymbols=false
dotnet publish --no-restore -p:Platform=x64 -r osx-x64 ./PrometheOSSkinEditor.csproj -c Release --self-contained true -p:DebugType=None -p:DebugSymbols=false