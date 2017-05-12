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
    public static bool IsPublishBuild { get; private set; }
    public static bool IsReleaseBuild { get; private set; }

    public static GitHubCredentials GitHub { get; private set; }
    public static MyGetCredentials MyGet { get; private set; }
    public static NuGetCredentials NuGet { get; private set; }
    public static AppVeyorCredentials AppVeyor { get; private set; }

    public static BuildVersion Version { get; private set; }
    public static BuildPaths Paths { get; private set; }
    
    public static DirectoryPath RootDirectoryPath { get; private set; }
    public static FilePath SolutionFilePath { get; private set; }
    public static DirectoryPath SourceDirectoryPath { get; private set; }
    public static DirectoryPath SolutionDirectoryPath { get; private set; }

    public static string Title { get; private set; }



    public static FilePath MilestoneReleaseNotesFilePath { get; private set; }
    public static FilePath FullReleaseNotesFilePath { get; private set; }

    public static bool ShouldPublishMyGet { get; private set; }
    public static bool ShouldPublishNuGet { get; private set; }
    public static bool ShouldPublishGitHub { get; private set; }
    public static bool ShouldGenerateDocumentation { get; private set; }
    public static bool ShouldExecuteGitLink { get; private set; }


    public static bool CanUseGitReleaseManager
    {
        get
        {
            return !string.IsNullOrEmpty(BuildParameters.GitHub.UserName) &&
                !string.IsNullOrEmpty(BuildParameters.GitHub.Password);
        }
    }


    public static void SetBuildVersion(BuildVersion version)
    {
        Version  = version;
    }

    public static void SetBuildPaths(BuildPaths paths)
    {
        Paths  = paths;
    }

    public static void PrintParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        context.Information("Printing Build Parameters...");
        context.Information("IsLocalBuild: {0}", IsLocalBuild);
        context.Information("IsPullRequest: {0}", IsPullRequest);
        context.Information("IsTagged: {0}", IsTagged);
        context.Information("IsMasterBranch: {0}", IsMasterBranch);


        
        context.Information("ShouldGenerateDocumentation: {0}", ShouldGenerateDocumentation);
        context.Information("ShouldExecuteGitLink: {0}", ShouldExecuteGitLink);
        context.Information("IsRunningOnUnix: {0}", IsRunningOnUnix);
        context.Information("IsRunningOnWindows: {0}", IsRunningOnWindows);
        context.Information("IsRunningOnAppVeyor: {0}", IsRunningOnAppVeyor);
    }

    public static void SetParameters(
        ICakeContext context,
        BuildSystem buildSystem,
        DirectoryPath sourceDirectoryPath,
        string title,
        FilePath solutionFilePath = null,
        DirectoryPath solutionDirectoryPath = null,
        DirectoryPath rootDirectoryPath = null,
        bool shouldPostToGitter = true,
        bool shouldPostToSlack = true,
        bool shouldPostToTwitter = true,
        bool shouldPostToMicrosoftTeams = false,
        FilePath milestoneReleaseNotesFilePath = null,
        FilePath fullReleaseNotesFilePath = null,
        bool shouldPublishMyGet = true,
        bool shouldPublishChocolatey = true,
        bool shouldPublishNuGet = true,
        bool shouldPublishGitHub = true,
        bool shouldGenerateDocumentation = true,
        bool shouldExecuteGitLink = true)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
        
        SourceDirectoryPath = sourceDirectoryPath;
        Title = title;
        SolutionFilePath = solutionFilePath ?? SourceDirectoryPath.CombineWithFilePath(Title + ".sln");
        SolutionDirectoryPath = solutionDirectoryPath ?? SourceDirectoryPath.Combine(Title);
        RootDirectoryPath = rootDirectoryPath ?? context.MakeAbsolute(context.Environment.WorkingDirectory);


        MilestoneReleaseNotesFilePath = milestoneReleaseNotesFilePath ?? RootDirectoryPath.CombineWithFilePath("CHANGELOG.md");
        FullReleaseNotesFilePath = fullReleaseNotesFilePath ?? RootDirectoryPath.CombineWithFilePath("ReleaseNotes.md");

        Target = context.Argument("target", "Default");
        Configuration = context.Argument("configuration", "Release");
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
        GitHub = GetGitHubCredentials(context);

        MyGet = GetMyGetCredentials(context);
        NuGet = GetNuGetCredentials(context);

        AppVeyor = GetAppVeyorCredentials(context);

        IsPublishBuild = new [] {
            "Create-Release-Notes"
        }.Any(
            releaseTarget => StringComparer.OrdinalIgnoreCase.Equals(releaseTarget, Target)
        );
        IsReleaseBuild = new [] {
            "Publish-NuGet-Packages",
            "Publish-Chocolatey-Packages",
            "Publish-GitHub-Release"
        }.Any(
            publishTarget => StringComparer.OrdinalIgnoreCase.Equals(publishTarget, Target)
        );

        SetBuildPaths(BuildPaths.GetPaths(context));

        ShouldPublishMyGet = (!IsLocalBuild &&
                                !IsPullRequest &&
                                (IsTagged || !IsMasterBranch) &&
                                shouldPublishMyGet);

        ShouldPublishNuGet = (!IsLocalBuild &&
                                !IsPullRequest &&
                                IsMasterBranch &&
                                IsTagged &&
                                shouldPublishNuGet);
        
        ShouldPublishGitHub = (!IsLocalBuild &&
                                !IsPullRequest &&
                                IsMasterBranch &&
                                IsTagged &&
                                shouldPublishGitHub);

        ShouldGenerateDocumentation = (!IsLocalBuild &&
                                !IsPullRequest &&
                                IsMasterBranch &&
                                shouldGenerateDocumentation);

        ShouldExecuteGitLink = (!IsLocalBuild && 
                            !IsPullRequest &&
                            IsMasterBranch &&
                            shouldExecuteGitLink);
    }
}