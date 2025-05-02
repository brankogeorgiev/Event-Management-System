#!/bin/bash

# Install .NET SDK 8.0 from a different download source (if the original is not working)
curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash -s -- --channel 8.0

# Update PATH to include the .NET SDK
export PATH=$PATH:$HOME/.dotnet

# Ensure dotnet is available
dotnet --version

# Restore dependencies
dotnet restore

# Build the application
dotnet build

# Publish the application
dotnet publish EMS.Web -c Release -o ./publish
