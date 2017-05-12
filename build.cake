//TODO Checkout https://github.com/cake-contrib/Cake.Recipe/blob/develop/build.cake
//#addin nuget:?package=Cake.Git&version=0.13.0

#load "./build/credentials.cake"
#load "./build/parameters.cake"
#load "./build/gitversion.cake"
#load "./build/environment.cake"
#load "./build/addins.cake"
#load "./build/paths.cake"
#load "./build/toolsettings.cake"



Environment.SetVariableNames();


var rootDirectoryPath         = MakeAbsolute(Context.Environment.WorkingDirectory);
var sourceDirectoryPath       = "./src";
var title                     = "Cake.Hosts";
var solutionFilePath          = "./src/Cake.Hosts.sln";


BuildParameters.SetParameters(Context, 
                            BuildSystem, 
                            sourceDirectoryPath, 
                            title, 
                            solutionFilePath: solutionFilePath,
                            shouldPostToGitter: false,
                            shouldPostToSlack: false,
                            shouldPostToTwitter: false,
                            shouldPublishChocolatey: false,
                            shouldPublishNuGet: false,
                            shouldPublishGitHub: false,
                            shouldGenerateDocumentation: false);

BuildParameters.PrintParameters(Context);
ToolSettings.SetToolSettings(Context);

var publishingError = false;


Setup(context =>
{
    if(BuildParameters.IsMasterBranch && (context.Log.Verbosity != Verbosity.Diagnostic)) {
        Information("Increasing verbosity to diagnostic.");
        context.Log.Verbosity = Verbosity.Diagnostic;
    }

    BuildParameters.SetBuildPaths(BuildPaths.GetPaths(Context));


    var semanticVersion = BuildVersion.CalculatingSemanticVersion(Context);
    BuildParameters.SetBuildVersion(semanticVersion);
    
    
    Information("Building version {0} of " + title + " ({1}, {2}) using version {3} of Cake. (IsTagged: {4})",
        BuildParameters.Version.SemVersion,
        BuildParameters.Configuration,
        BuildParameters.Target,
        BuildParameters.Version.CakeVersion,
        BuildParameters.IsTagged);
});




Task("Debug")
    .Does(() => 
    {
        Information("Target: {0}", BuildParameters.Target);
        Information("Configuration: {0}", BuildParameters.Configuration);

        Information("Source DirectoryPath: {0}", MakeAbsolute(BuildParameters.SourceDirectoryPath));
        Information("Build DirectoryPath: {0}", MakeAbsolute(BuildParameters.Paths.Directories.Build));
    });

Task("Clean")
    .Does(() =>
{
    Information("Cleaning...");

    CleanDirectories(BuildParameters.Paths.Directories.ToClean);
});


RunTarget(BuildParameters.Target);