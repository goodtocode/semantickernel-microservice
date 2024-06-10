using System.IO;
using System.Reflection;

namespace $safeprojectname$
{
    public static class PathFactory
    {
        public static string GetProjectSubfolder(string folderName)
        {
            // Visual Studio vs. dotnet test vs. test runtimes execute different folders            
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            executingPath = Directory.Exists($"{executingPath}/{folderName}") ? executingPath : Directory.GetCurrentDirectory();
            executingPath = Directory.Exists($"{executingPath}/{folderName}") ? executingPath : $"{Directory.GetParent(executingPath)}/bin/Debug/net5.0";
            return $"{executingPath}/{folderName}";
        }
    }
}
