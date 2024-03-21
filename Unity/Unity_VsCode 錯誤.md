# Unity_VsCode 錯誤

紀錄一些常見的錯誤

## csproj 錯誤

<https://blog.csdn.net/JavaD0g/article/details/132517570>

如果出現以下訊息
>[warning] The project file ‘d:\Unity\Project\Unity.Services.Core.Configuration.csproj’ is in unsupported format (for example, a traditional .NET Framework project). It need be converted to new SDK style to work in C# Dev Kit.
>[error] Failed to load project ‘d:\Unity\Project\Unity.Services.Core.Configuration.csproj’. One or more errors occurred. (This project is not supported in C# Dev Kit.)

原因是 `VsCode` 與專案的差件版本不符 可以以下修復
1. 更新 Visual Studio Code Editor / Visual Studio Editor
   > Windows> Package Manager
2. 重新產生 `.csript`
   > Edit> Preferences> Extrmal Tools


