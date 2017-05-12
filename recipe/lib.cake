Action<string, IDictionary<string, string>> RequireAddin = (code, envVars) => {
    var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));
    try
    {
        System.IO.File.WriteAllText(script.FullPath, code);
        CakeExecuteScript(script, new CakeSettings{ EnvironmentVariables = envVars });
    }
    finally
    {
        if (FileExists(script))
        {
            DeleteFile(script);
        }
    }
};


Action<string, Action> RequireTool = (tool, action) => {
    var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));
    try
    {
        System.IO.File.WriteAllText(script.FullPath, tool);
        CakeExecuteScript(script);
    }
    finally
    {
        if (FileExists(script))
        {
            DeleteFile(script);
        }
    }

    action();
};


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

public static MyGetCredentials GetMyGetCredentials(ICakeContext context)
{
    return new MyGetCredentials(
        context.EnvironmentVariable(Environment.MyGetApiKeyVariable),
        context.EnvironmentVariable(Environment.MyGetSourceUrlVariable));
}

public static class Environment
{
    public static string MyGetApiKeyVariable { get; private set; }
    public static string MyGetSourceUrlVariable { get; private set; }

    public static void SetVariableNames(
        string myGetApiKeyVariable = null,
        string myGetSourceUrlVariable = null)
    {
        MyGetApiKeyVariable = myGetApiKeyVariable ?? "MYGET_API_KEY";
        MyGetSourceUrlVariable = myGetSourceUrlVariable ?? "MYGET_SOURCE";
    }
}