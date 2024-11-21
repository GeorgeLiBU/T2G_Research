using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace T2G.UnityAdapter
{

    public class ContentLibrary
    {
        public static bool ImportScript(string scriptName, string dependencies)
        {
            bool retVal = true;
            string scriptsFolderName = "Scripts";
            var sourcePath = Path.Combine(Settings.RecoursePath, scriptsFolderName, scriptName);
            var destDir = Path.Combine(Application.dataPath, scriptsFolderName);
            var destPath = Path.Combine(destDir, scriptName);
            if (File.Exists(sourcePath))
            {
                if (File.Exists(destPath))
                {
                    retVal = true;
                }
                else
                {
                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir);
                    }
                    File.Copy(sourcePath, destPath);
                    var importPath = Path.Combine("Assets", scriptsFolderName, scriptName);
                    var dependencyArray = dependencies.Split(',');
                    foreach (var dependency in dependencyArray)
                    {
                        sourcePath = Path.Combine(Settings.RecoursePath, scriptsFolderName, dependency);
                        destPath = Path.Combine(Application.dataPath, scriptsFolderName, dependency);
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, destPath);
                        }
                    }
                    AssetDatabase.Refresh();
                    CompilationPipeline.compilationFinished += (obj) =>
                    {

                    };
                    CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.CleanBuildCache);
                    //CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.None);
                }
            }
            else
            {
                retVal = false;
            }

            return retVal;
        }

        public static bool ImportPrefab(string prefabName)
        {
            return true;
        }

    }
}
