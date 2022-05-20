using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BakedData
{
    public string Name { get; private set; }
    public float ClipLength { get; private set; }
    public byte[] RawTextureData { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    public BakedData(string name, float clipLength, Texture2D animationMap)
    {
        Name = name;
        ClipLength = clipLength;
        Width = animationMap.width;
        Height = animationMap.height;
        RawTextureData = animationMap.GetRawTextureData();
    }
}
