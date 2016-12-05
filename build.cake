var target = Argument("target", "Default");

Task("Restore")
  .Does(() =>
{
    DotNetCoreRestore("src/");
});

Task("Build")
    .IsDependentOn("Restore")
  .Does(() =>
{
    DotNetCoreBuild("src/**/project.json");
});

Task("Pack")
    .IsDependentOn("Build")
  .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "publish/"
    };

    var files = GetFiles("src/**/project.json");
    foreach(var file in files)
    {
        DotNetCorePack(file.ToString(), settings);
    } 
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");

RunTarget(target);