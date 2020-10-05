#ASP.NET Core LibMan Library Update Checker

[LibMan](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/) only allows you to update a single library at a time so I decided to build a tool that checks all libraries in libman.json for updates.
You'll still need to update each one manually but it should save some time since it will list all libraries with updates.

CDNs Supported: (same as LibMan) CDNJS, jsDelivr, and unpkg.