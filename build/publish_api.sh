#!/bin/bash
dotnet publish ../src/Jahshaka.API -o ../../build/artifacts/jahshaka-api -f netcoreapp1.1 -c Debug
echo Press enter to continue; read dummy;
