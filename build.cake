// https://github.com/cake-contrib/Cake.Recipe/ used as a template
#load "./recipe/lib.cake"
#load "./recipe/parameters.cake"
#load "./recipe/gitversion.cake"
#load "./recipe/appveyor.cake"



Environment.SetVariableNames(
    myGetApiKeyVariable: "CAKE_HOSTS_MYGET_API_KEY",
    myGetSourceUrlVariable: "CAKE_HOSTS_MYGET_SOURCE"
);


var rootDirectoryPath         = MakeAbsolute(Context.Environment.WorkingDirectory);
var title                     = "Cake.Hosts";
var sourceDirectoryPath       = "./src";
var solutionDirectoryPath     = "./src";
var solutionFilePath          = "./src/Cake.Hosts.sln";
var solutionInfoFilePath      = "./src/Cake.Hosts/SolutionInfo.cs";
var projectDirectoryPath      = "./src/Cake.Hosts/";
var projectFilePath           = "./src/Cake.Hosts/Cake.Hosts.csproj";
var testsProjFilePath         = "./src/Cake.Hosts.Tests/Cake.Hosts.Tests.csproj";


BuildParameters.SetParameters(Context, 
                            BuildSystem, 
                            sourceDirectoryPath, 
                            title, 
                            solutionFilePath: solutionFilePath,
                            solutionInfoFilePath: solutionInfoFilePath,
                            solutionDirectoryPath: solutionDirectoryPath,
                            projectDirectoryPath: projectDirectoryPath,
                            projectFilePath: projectFilePath,
                            testsProjFilePath: testsProjFilePath,
                            shouldPublishMyGet: true);


var publishingError = false;


Setup(context =>
{
    if(BuildParameters.IsMasterBranch && (context.Log.Verbosity != Verbosity.Diagnostic)) {
        Information("Increasing verbosity to diagnostic.");
        context.Log.Verbosity = Verbosity.Diagnostic;
    }

    var semanticVersion = BuildVersion.CalculatingSemanticVersion(Context);
    BuildParameters.SetBuildVersion(semanticVersion);
});


Task("Debug")
    .IsDependentOn("Show-Info");

Task("Show-Info")
    .IsDependentOn("Print-AppVeyor-Environment-Variables")
    .Does(() => 
    {
        Information("Building version {0} of {5} ({1}, {2}) using version {3} of Cake. (IsTagged: {4})",
            BuildParameters.Version.SemVersion,
            BuildParameters.Configuration,
            BuildParameters.Target,
            BuildParameters.Version.CakeVersion,
            BuildParameters.IsTagged, 
            title);
        
        Information("Target: {0}", BuildParameters.Target);
        Information("Configuration: {0}", BuildParameters.Configuration);
        Information("Source DirectoryPath: {0}", MakeAbsolute(BuildParameters.SourceDirectoryPath));
        Information("Build DirectoryPath: {0}", MakeAbsolute(BuildParameters.BuildArtifacts));
        BuildParameters.PrintParameters(Context);
    });

Task("Clean")
    .Does(() =>
    {
        Information("Cleaning...");

        CleanDirectory(BuildParameters.BuildArtifacts);
        CleanDirectories("./src/**/bin/**");
        CleanDirectories("./src/**/obj/**");
    });



Task("Restore")
    .Does(() =>
    {
        Information("Restoring {0}...", BuildParameters.SolutionFilePath);

        var settings = new DotNetCoreRestoreSettings
        {
            Sources = new[] { "https://www.nuget.org/api/v2" },
            DisableParallel = false,
            WorkingDirectory = sourceDirectoryPath,
        };

        DotNetCoreRestore(settings);
    });


Task("Build")
    .IsDependentOn("Show-Info")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => 
    {
        Information("Building {0}", BuildParameters.SolutionFilePath);

        var settings = new DotNetCoreBuildSettings
        {
            Configuration = BuildParameters.Configuration,
        };

        DotNetCoreBuild(sourceDirectoryPath, settings);
    });


Task("Tests")
    .IsDependentOn("Build")
    .Does(() => 
    {
        Information("Testing {0}", BuildParameters.SolutionFilePath);

        var settings = new DotNetCoreTestSettings
        {
            Configuration = BuildParameters.Configuration,
        };

        DotNetCoreTest(BuildParameters.TestsProjFilePath.FullPath, settings);
    });


Task("Pack")
    .IsDependentOn("Package");

Task("Package")
    .IsDependentOn("Tests")
    .Does(() => 
    {
        Information("Packing {0}", BuildParameters.SolutionFilePath);

        var settings = new DotNetCorePackSettings
        {
            Configuration = BuildParameters.Configuration,
            OutputDirectory = BuildParameters.BuildArtifacts,
            NoBuild = true, // should already be built
            ArgumentCustomization = args=>args.Append("/p:Version="+BuildParameters.Version.Version),
        };

        DotNetCorePack(BuildParameters.ProjectFilePath.FullPath, settings);
    });


Task("Publish-MyGet-Packages")
    .IsDependentOn("Package")
    .WithCriteria(() => BuildParameters.ShouldPublishMyGet)
    .WithCriteria(() => DirectoryExists(BuildParameters.BuildArtifacts))
    .Does(() =>
{
    if(string.IsNullOrEmpty(BuildParameters.MyGet.ApiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    if(string.IsNullOrEmpty(BuildParameters.MyGet.SourceUrl)) {
        throw new InvalidOperationException("Could not resolve MyGet API url.");
    }

    var nupkgFiles = GetFiles(BuildParameters.BuildArtifacts + "/**/*.nupkg");

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


Task("Default")
    .IsDependentOn("Package");

Task("AppVeyor")
    .IsDependentOn("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Publish-MyGet-Packages")
    .Finally(() =>
{
    if(publishingError)
    {
        throw new Exception("An error occurred during the publishing of " + BuildParameters.Title + ".  All publishing tasks have been attempted.");
    }
});

RunTarget(BuildParameters.Target);