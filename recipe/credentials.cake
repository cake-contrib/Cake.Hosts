public class GitHubCredentials
{
    public string UserName { get; private set; }
    public string Password { get; private set; }

    public GitHubCredentials(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}

public class MyGetCredentials
{
    public string ApiKey { get; private set; }
    public string SourceUrl { get; private set; }

    public MyGetCredentials(string apiKey, string sourceUrl)
    {
        ApiKey = apiKey;
        SourceUrl = sourceUrl;
    }
}

public class NuGetCredentials
{
    public string ApiKey { get; private set; }
    public string SourceUrl { get; private set; }

    public NuGetCredentials(string apiKey, string sourceUrl)
    {
        ApiKey = apiKey;
        SourceUrl = sourceUrl;
    }
}
public class AppVeyorCredentials
{
    public string ApiToken { get; private set; }

    public AppVeyorCredentials(string apiToken)
    {
        ApiToken = apiToken;
    }
}

public class CoverallsCredentials
{
    public string RepoToken { get; private set; }

    public CoverallsCredentials(string repoToken)
    {
        RepoToken = repoToken;
    }
}


public static GitHubCredentials GetGitHubCredentials(ICakeContext context)
{
    return new GitHubCredentials(
        context.EnvironmentVariable(Environment.GithubUserNameVariable),
        context.EnvironmentVariable(Environment.GithubPasswordVariable));
}

public static MyGetCredentials GetMyGetCredentials(ICakeContext context)
{
    return new MyGetCredentials(
        context.EnvironmentVariable(Environment.MyGetApiKeyVariable),
        context.EnvironmentVariable(Environment.MyGetSourceUrlVariable));
}

public static NuGetCredentials GetNuGetCredentials(ICakeContext context)
{
    return new NuGetCredentials(
        context.EnvironmentVariable(Environment.NuGetApiKeyVariable),
        context.EnvironmentVariable(Environment.NuGetSourceUrlVariable));
}


public static AppVeyorCredentials GetAppVeyorCredentials(ICakeContext context)
{
    return new AppVeyorCredentials(
        context.EnvironmentVariable(Environment.AppVeyorApiTokenVariable));
}

public static CoverallsCredentials GetCoverallsCredentials(ICakeContext context)
{
    return new CoverallsCredentials(
        context.EnvironmentVariable(Environment.CoverallsRepoTokenVariable));
}