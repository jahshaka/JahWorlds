#!/bin/bash
dotnet restore ../src/Jahshaka.Admin
dotnet publish ../src/Jahshaka.Admin -o ../../build/artifacts/jahshaka-admin -f netcoreapp2.0 -c Debug /p:Env=dev
echo Press enter to continue; read dummy;