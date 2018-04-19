#!/bin/bash
dotnet restore ../src/Jahshaka.API
dotnet publish ../src/Jahshaka.API -o ../../build/artifacts/jahshaka-api -f netcoreapp2.0 -c Debug
echo Press enter to continue; read dummy;

