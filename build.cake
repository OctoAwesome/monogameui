// *********************
//      ARGUMENTS
// *********************
var Target = Argument("target", "default");
var Configuration = Argument("configuration", "release");

var Solution = File("./MonoGameUi.sln");

// *********************
//      TASKS
// *********************

Task("build")
    .Does(() => DotNetBuild(Solution));

Task("clean")
    .Does(() => 
    {
        CleanDirectories("./output");
        CleanDirectories(string.Format("./src/**/obj/{0}", Configuration));
        CleanDirectories(string.Format("./src/**/bin/{0}", Configuration));
        CleanDirectories(string.Format("./tests/**/obj/{0}", Configuration));
        CleanDirectories(string.Format("./tests/**/bin/{0}", Configuration));
    });

Task("default")
    .IsDependentOn("restore")
    .IsDependentOn("clean")
    .IsDependentOn("build");

Task("restore")
    .Does(() => NuGetRestore(Solution));

// EXECUTE
RunTarget(Target);