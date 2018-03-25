#tool "nuget:?package=xunit.runner.console&version=2.2.0"

var target = Argument("target", "Default");
var configuration = Argument("Configuration", "Release");  
var solution = "./cs-ilp-core.sln";

Task("Restore")  
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("Build")  
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetCoreBuild(
            solution, 
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            });
    });

Task("Test")  
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./*test/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTest(
                project.FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    Verbosity = DotNetCoreVerbosity.Normal,
                    DiagnosticOutput = true,
                    ResultsDirectory = new DirectoryPath("./TestResults")
                });
        }
    });

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);