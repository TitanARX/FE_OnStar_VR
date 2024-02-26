using System.IO;
using UnityEditor;
using UnityEngine;

public class ProjectSetup : EditorWindow
{
    [MenuItem("Custom/Setup Project")]
    private static void SetupProject()
    {
        CreateFolders();
    }

    private static void CreateFolders()
    {
        string projectPath = "Assets/_Project Repo";

        // Create main project folder
        if (!AssetDatabase.IsValidFolder(projectPath))
        {
            AssetDatabase.CreateFolder("Assets", "_Project Repo");
        }

        // List of all folders
        string[] folders = {
            "3rd-Party",
            "Animations",
            "Animations/2DAnimations",
            "Animations/3DAnimations",
            "Audio",
            "Music",
            "SFX",
            "Materials",
            "Models",
            "Plugins",
            "Prefabs",
            "Resources",
            "Textures",
            "Sandbox",
            "Scenes",
            "Scenes/Levels",
            "Scenes/Others",
            "Other",
            "Scripts",
            "Scripts/Editor",
            "Shaders"
        };

        // Create subfolders
        foreach (string folderName in folders)
        {
            CreateSubfolder(projectPath, folderName);
        }
    }

    private static void CreateSubfolder(string parentFolder, string folderName)
    {
        string path = Path.Combine(parentFolder, folderName);
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parentFolder, folderName);
        }
    }
}
