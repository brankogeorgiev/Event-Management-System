#!/bin/bash

# Install .NET SDK 8.0
curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash -s -- --version 8.0

# Update PATH to include the .NET SDK
export PATH=$PATH:$HOME/.dotnet

# Ensure dotnet command is available
dotnet --version

# Restore dependencies
dotnet restore

# Build the app
dotnet build

# Publish the app for deployment
dotnet publish EMS.Web -c Release -o ./publish
