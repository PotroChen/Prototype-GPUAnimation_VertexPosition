using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimationBakerWindow : EditorWindow
{
    [MenuItem("AnimationBaker/Bake")]
    public static void Bake()
    {
        string[] guids = Selection.assetGUIDs;

        for (int i = 0; i < guids.Length; i++)
        {
            string guid = guids[i];
            string path = AssetDatabase.GUIDToAssetPath(guid);

            string folderPath = path.Substring(0,path.LastIndexOf("/")+1);

            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (gameObject == null)
                continue;

            AnimationMapBaker baker = new AnimationMapBaker();
            baker.GenerateRawData(gameObject);
            var bakedDataList = baker.Bake();

            foreach (var bakedData in bakedDataList)
            {
                SaveTexture(bakedData, folderPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void SaveTexture(BakedData data,string parentPath)
    {
        string folderPath = parentPath + "AnimationMap";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        Texture2D animationMap = new Texture2D(data.Width, data.Height, TextureFormat.RGBAHalf, false);
        animationMap.LoadRawTextureData(data.RawTextureData);
        AssetDatabase.CreateAsset(animationMap, Path.Combine(folderPath, data.Name + ".asset"));
    }
}
