#!/bin/bash
dotnet publish ../src/Jahshaka.AuthServer -o ../../build/artifacts/jahshaka-auth-server -f netcoreapp1.1 -c Debug
echo Press enter to continue; read dummy;
