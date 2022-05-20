using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct RawDataPerRenderer
{
    public string Name { get; private set; }
    public int MapWidth { get; private set; }

    public SkinnedMeshRenderer Renderer { get;private set; }
    private int vertexCount;

    public RawDataPerRenderer(SkinnedMeshRenderer renderer)
    {
        Name = renderer.name;
        this.Renderer = renderer;
        vertexCount = renderer.sharedMesh.vertexCount;
        MapWidth = Mathf.NextPowerOfTwo(vertexCount);
    }
}

public struct RawDataPerAnimation
{
    public string Name { get; private set; }
    
    public Animation Animation { get; private set; }

    public List<AnimationState> AnimationStates { get; private set; }

    public RawDataPerRenderer[] RawDataPerRenderers { get; private set; }

    public RawDataPerAnimation(string name, Animation animation,SkinnedMeshRenderer[] renderers)
    {
        Name = name;
        Animation = animation;
        AnimationStates = new List<AnimationState>(animation.Cast<AnimationState>());
        RawDataPerRenderers = new RawDataPerRenderer[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            RawDataPerRenderers[i] = new RawDataPerRenderer(renderers[i]);
        }
    }
}
