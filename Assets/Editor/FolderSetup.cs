using UnityEngine;
using UnityEditor;
using System.IO;

public class FolderSetup : EditorWindow
{
    [MenuItem("Tools/Create Project Folders")]
    public static void CreateFolders()
    {
        string[] folders = new string[]
        {
            "Assets/Scenes",
            "Assets/Prefabs",
            "Assets/Materials",
            "Assets/Textures",
            "Assets/Scripts",
            "Assets/Scripts/Managers",
            "Assets/Scripts/Player",
            "Assets/Scripts/Enemies",
            "Assets/Scripts/Data",
            "Assets/Scripts/Utils",
            "Assets/ScriptableObjects",
            "Assets/UI",
            "Assets/Addressables"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Debug.Log("Created: " + folder);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Project folder structure created!");
    }
}
