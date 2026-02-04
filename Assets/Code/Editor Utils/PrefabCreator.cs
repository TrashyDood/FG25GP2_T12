using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabCreator
{
    [MenuItem("Assets/Create/New Body Part")]
    public static void CreateNewBodyPartAsset()
    {
        // Load your source prefab
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/Prefabs/Bodyparts/Empty Body Part.prefab");

        if (sourcePrefab == null)
        {
            Debug.LogError("Source prefab not found! Check the path.");
            return;
        }

        // Get the selected folder path in Project window
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }

        // Create a unique path for the new prefab
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Body Part.prefab");

        // Copy the prefab to the new location
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(sourcePrefab), assetPathAndName);

        // Select the newly created prefab
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPathAndName);
    }
}