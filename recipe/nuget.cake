Task("Publish-MyGet-Packages")
    .IsDependentOn("Package")
    .WithCriteria(() => BuildParameters.ShouldPublishMyGet)
    .WithCriteria(() => DirectoryExists(BuildParameters.Paths.Directories.NuGetPackages) || DirectoryExists(BuildParameters.Paths.Directories.ChocolateyPackages))
    .Does(() =>
{
    if(string.IsNullOrEmpty(BuildParameters.MyGet.ApiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    if(string.IsNullOrEmpty(BuildParameters.MyGet.SourceUrl)) {
        throw new InvalidOperationException("Could not resolve MyGet API url.");
    }

    var nupkgFiles = GetFiles(BuildParameters.Paths.Directories.NuGetPackages + "/**/*.nupkg");

    foreach(var nupkgFile in nupkgFiles)
    {
        // Push the package.
        NuGetPush(nupkgFile, new NuGetPushSettings {
            Source = BuildParameters.MyGet.SourceUrl,
            ApiKey = BuildParameters.MyGet.ApiKey
        });
    }

    nupkgFiles = GetFiles(BuildParameters.Paths.Directories.ChocolateyPackages + "/**/*.nupkg");

    foreach(var nupkgFile in nupkgFiles)
    {
        // Push the package.
        NuGetPush(nupkgFile, new NuGetPushSettings {
            Source = BuildParameters.MyGet.SourceUrl,
            ApiKey = BuildParameters.MyGet.ApiKey
        });
    }
})
.OnError(exception =>
{
    Error(exception.Message);
    Information("Publish-MyGet-Packages Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Publish-Nuget-Packages")
    .IsDependentOn("Package")
    .WithCriteria(() => BuildParameters.ShouldPublishNuGet)
    .WithCriteria(() => DirectoryExists(BuildParameters.Paths.Directories.NuGetPackages))
    .WithCriteria(() => false) // never publish nuget packages from here
    .Does(() =>
{
    if(string.IsNullOrEmpty(BuildParameters.NuGet.ApiKey)) {
        throw new InvalidOperationException("Could not resolve NuGet API key.");
    }

    if(string.IsNullOrEmpty(BuildParameters.NuGet.SourceUrl)) {
        throw new InvalidOperationException("Could not resolve NuGet API url.");
    }

    var nupkgFiles = GetFiles(BuildParameters.Paths.Directories.NuGetPackages + "/**/*.nupkg");

    foreach(var nupkgFile in nupkgFiles)
    {
        // Push the package.
        NuGetPush(nupkgFile, new NuGetPushSettings {
            Source = BuildParameters.NuGet.SourceUrl,
            ApiKey = BuildParameters.NuGet.ApiKey
        });
    }
})
.OnError(exception =>
{
    Error(exception.Message);
    Information("Publish-Nuget-Packages Task failed, but continuing with next Task...");
    publishingError = true;
});