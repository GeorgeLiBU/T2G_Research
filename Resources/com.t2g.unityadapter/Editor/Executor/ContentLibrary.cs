using System.IO;
using UnityEditor;
using UnityEngine;

namespace T2G.UnityAdapter
{

    public class ContentLibrary
    {
        public static bool ImportScript(string scriptName, string dependencies)
        {
            bool retVal = true;
            var sourcePath = Path.Combine(Settings.RecoursePath, "Scripts", scriptName);
            var destDir = Path.Combine(Application.dataPath, "Scripts");
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
                    var importPath = Path.Combine("Assets", "Scripts", scriptName);
                    var dependencyArray = dependencies.Split(',');
                    foreach (var dependency in dependencyArray)
                    {
                        sourcePath = Path.Combine(Settings.RecoursePath, "Scripts", dependency);
                        destPath = Path.Combine(Application.dataPath, "Scripts", dependency);
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, destPath);
                        }
                    }
                    AssetDatabase.Refresh();
                    AssetDatabase.ImportAsset(importPath);
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
