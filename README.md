# ASP.NET Core LibMan Library Update Checker

[LibMan](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/) only allows you to update a single library at a time so I decided to build a tool that checks all libraries in libman.json for updates. You'll still need to update each one manually but it should save some time since it will only list the libraries with updates.

## CDNs supported
CDNJS, jsDelivr, and Unpkg (same as LibMan).

### CDNJS
Returns the latest version (may be stable or beta).

### jsDelivr
Returns the latest stable version.

### Unpkg
Returns the latest version (may be stable or beta).

## Supported operating systems
Only Windows as of now.

## How to run
