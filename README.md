# Starsheet - Sample Unity Project

Example intergration of [Starsheet](https://starsheet.app) into a Unity project. This example is designed to be as simple as possible rather than be fully production ready. 

This example demonstrates the following: 

* Allows multiple Starsheet projects to be used within a single Unity project
* Data responses are cached locally. Uses `If-None-Match` on subsequent requests to avoid redownloading the same data
* Images are cached. Only new/changed images will be downloaded even if data is updated

Please note the following limitations of the sample project: 

* Old images are not removed from the cache when they are no longer references.

# Overview

The Starsheet download is initiated in [Assets/Scripts/Splash.cs](https://github.com/Starsheet/unity-sample/blob/main/Assets/Scripts/Splash.cs), which calls [StarsheetDownloader](https://github.com/Starsheet/unity-sample/blob/main/Assets/Scripts/Starsheet/StarsheetDownloader.cs)

## Dependencies

This project was build in `Unity 2021.3.44`. 

* Newtonsoft JSON ([com.unity.nuget.newtonsoft-json@3.2](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html))
