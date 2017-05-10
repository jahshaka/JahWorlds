#!/bin/bash
dotnet publish ../src/Jahshaka.Web -o ../../build/artifacts/jahshaka-web -f netcoreapp1.1 -c Debug
echo Press enter to continue; read dummy;
