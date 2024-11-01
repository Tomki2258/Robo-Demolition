using UnityEngine;
using UnityEditor;

public class PNGToMaterials : MonoBehaviour
{
    [MenuItem("Tools/Convert PNG to Materials")]
    static void ConvertPNGToMaterials()
    {
        string path = EditorUtility.OpenFolderPanel("Select PNG Folder", "", "");
        if (path.Length != 0)
        {
            string[] files = System.IO.Directory.GetFiles(path, "*.png");
            foreach (string file in files)
            {
                string assetPath = "Assets" + file.Substring(Application.dataPath.Length);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (texture != null)
                {
                    Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    material.mainTexture = texture;
                    string materialPath = assetPath.Replace(".png", ".mat");
                    AssetDatabase.CreateAsset(material, materialPath);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}