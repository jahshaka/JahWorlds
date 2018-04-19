#!/bin/bash
dotnet restore ../src/Jahshaka.AuthServer
dotnet publish ../src/Jahshaka.AuthServer -o ../../build/artifacts/jahshaka-auth-server -f netcoreapp2.0 -c Debug
echo Press enter to continue; read dummy;
