public static class Environment
{
    public static string GithubUserNameVariable { get; private set; }
    public static string GithubPasswordVariable { get; private set; }
    public static string MyGetApiKeyVariable { get; private set; }
    public static string MyGetSourceUrlVariable { get; private set; }
    public static string NuGetApiKeyVariable { get; private set; }
    public static string NuGetSourceUrlVariable { get; private set; }

    public static string AppVeyorApiTokenVariable { get; private set; }

    public static void SetVariableNames(
        string githubUserNameVariable = null,
        string githubPasswordVariable = null,
        string myGetApiKeyVariable = null,
        string myGetSourceUrlVariable = null, 
        string nuGetApiKeyVariable = null,
        string nuGetSourceUrlVariable = null, 
        string appVeyorApiTokenVariable = null)
    {
        GithubUserNameVariable = githubUserNameVariable ?? "GITHUB_USERNAME";
        GithubPasswordVariable = githubPasswordVariable ?? "GITHUB_PASSWORD";
        MyGetApiKeyVariable = myGetApiKeyVariable ?? "MYGET_API_KEY";
        MyGetSourceUrlVariable = myGetSourceUrlVariable ?? "MYGET_SOURCE";
        NuGetApiKeyVariable = nuGetApiKeyVariable ?? "NUGET_API_KEY";
        NuGetSourceUrlVariable = nuGetSourceUrlVariable ?? "NUGET_SOURCE";

        AppVeyorApiTokenVariable = appVeyorApiTokenVariable ?? "APPVEYOR_API_TOKEN";
    }
}