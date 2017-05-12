public static class BuildParameters
{
    public static string Target { get; private set; }
    public static string Configuration { get; private set; }
    public static bool IsLocalBuild { get; private set; }
    public static bool IsRunningOnUnix { get; private set; }
    public static bool IsRunningOnWindows { get; private set; }
    public static bool IsRunningOnAppVeyor { get; private set; }
    public static bool IsPullRequest { get; private set; }
    public static bool IsMasterBranch { get; private set; }
    public static bool IsTagged { get; private set; }

    public static MyGetCredentials MyGet { get; private set; }
    public static BuildVersion Version { get; private set; }
    
    public static string Title { get; private set; }
    public static DirectoryPath RootDirectoryPath { get; private set; }
    public static FilePath SolutionFilePath { get; private set; }
    public static FilePath SolutionInfoFilePath { get; private set; }

    public static FilePath ProjectFilePath { get; private set; }
    public static FilePath TestsProjFilePath { get; private set; }

    public static DirectoryPath SourceDirectoryPath { get; private set; }
    public static DirectoryPath SolutionDirectoryPath { get; private set; }
    public static DirectoryPath ProjectDirectoryPath { get; private set; }
    public static DirectoryPath BuildArtifacts { get; private set; }


    public static bool ShouldPublishMyGet { get; private set; }

    public static void SetBuildVersion(BuildVersion version)
    {
        Version  = version;
    }

    public static void PrintParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        context.Information("Printing Build Parameters...");

        context.Information("Target: {0}", Target);
        context.Information("Configuration: {0}", Configuration);
        context.Information("IsLocalBuild: {0}", IsLocalBuild);
        context.Information("IsPullRequest: {0}", IsPullRequest);
        context.Information("IsTagged: {0}", IsTagged);
        context.Information("IsMasterBranch: {0}", IsMasterBranch);
        context.Information("IsRunningOnUnix: {0}", IsRunningOnUnix);
        context.Information("IsRunningOnWindows: {0}", IsRunningOnWindows);
        context.Information("IsRunningOnAppVeyor: {0}", IsRunningOnAppVeyor);
        context.Information("Version.Version: {0}", Version.Version);
        context.Information("Version.SemVersion: {0}", Version.SemVersion);
        context.Information("Version.Milestone: {0}", Version.Milestone);
        context.Information("Version.CakeVersion: {0}", Version.CakeVersion);

    }

    public static void SetParameters(
        ICakeContext context,
        BuildSystem buildSystem,
        DirectoryPath sourceDirectoryPath,
        string title,
        FilePath solutionFilePath = null,
        FilePath solutionInfoFilePath = null,
        FilePath projectFilePath = null,
        FilePath testsProjFilePath = null,
        DirectoryPath solutionDirectoryPath = null,
        DirectoryPath rootDirectoryPath = null,
        DirectoryPath projectDirectoryPath = null,
        bool shouldPublishMyGet = true)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
        Target = context.Argument("target", "Default");
        Configuration = context.Argument("configuration", "Release");
        BuildArtifacts = "./BuildArtifacts";
        
        SourceDirectoryPath = sourceDirectoryPath;
        Title = title;
        SolutionFilePath = solutionFilePath ?? SourceDirectoryPath.CombineWithFilePath(Title + ".sln");
        SolutionInfoFilePath = solutionInfoFilePath ?? SourceDirectoryPath.CombineWithFilePath("SolutionInfo.cs");
        ProjectFilePath = projectFilePath ?? SourceDirectoryPath.CombineWithFilePath(Title + "/" + Title + ".csproj");
        TestsProjFilePath = testsProjFilePath ?? SourceDirectoryPath.CombineWithFilePath(Title + ".Tests/" + Title + ".Tests.csproj");

        SolutionDirectoryPath = solutionDirectoryPath ?? SourceDirectoryPath.Combine(Title);
        RootDirectoryPath = rootDirectoryPath ?? context.MakeAbsolute(context.Environment.WorkingDirectory);
        ProjectDirectoryPath = projectDirectoryPath ?? SourceDirectoryPath.Combine(Title);


        IsLocalBuild = buildSystem.IsLocalBuild;
        IsRunningOnUnix = context.IsRunningOnUnix();
        IsRunningOnWindows = context.IsRunningOnWindows();
        IsRunningOnAppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;

        IsMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildSystem.AppVeyor.Environment.Repository.Branch);
        
        IsTagged = (
            buildSystem.AppVeyor.Environment.Repository.Tag.IsTag &&
            !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name)
        );

        MyGet = GetMyGetCredentials(context);
        ShouldPublishMyGet = (!IsPullRequest && shouldPublishMyGet);
    }
}