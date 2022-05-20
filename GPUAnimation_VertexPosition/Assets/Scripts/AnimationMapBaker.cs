using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��ͼ���� = �������� * SkinnedMeshRender����
public class AnimationMapBaker
{
    public static bool includeInactive = true;

    private RawDataPerAnimation? rawData = null;
    private List<BakedData> bakedDataList = new List<BakedData>();

    private Mesh tempMesh;

    public void GenerateRawData(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log("GameObject is null");
            return;
        }

        Animation animation = gameObject.GetComponent<Animation>();
        SkinnedMeshRenderer[] renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive);

        if (animation == null || renderers == null || renderers.Length == 0)
        {
            Debug.Log("No animation or renderers found on GameObject");
            return;
        }

        tempMesh = new Mesh();
        rawData = new RawDataPerAnimation(gameObject.name, animation, renderers);
    }

    public List<BakedData> Bake()
    {
        if (rawData == null)
        {
            Debug.LogError("RawData Needed");
            return bakedDataList;
        }

        //��ͼ���� = �������� * renderer����
        foreach (var animationState in rawData.Value.AnimationStates)
        {
            if (animationState.clip == null)
                continue;
            if (!animationState.clip.legacy)
            {
                Debug.LogError(string.Format("Animation Clip [{0}] is not legacy", animationState.clip.name));
                continue;
            }
            for (int i = 0; i < rawData.Value.RawDataPerRenderers.Length; i++)
            {
                BakePerRenderer(animationState, rawData.Value, i);
            }
        }
        return bakedDataList;
    }

    private void BakePerRenderer(AnimationState animationState, RawDataPerAnimation rawData,int subIndex)
    {
        RawDataPerRenderer subRawData = rawData.RawDataPerRenderers[subIndex];

        //����֡�� = ֡�� * ʱ�䣬��֤��2���ݴη������������,���ܻᵼ��һ��㶯����Ϣ��ʧ
        int frameCount = Mathf.ClosestPowerOfTwo((int)(animationState.clip.frameRate * animationState.length));
        float timePerFrame = animationState.length / frameCount;

        Texture2D animationMap = new Texture2D(subRawData.MapWidth, frameCount, TextureFormat.RGBAHalf, true);
        animationMap.name = string.Format("{0}_{1}_{2}", rawData.Name, subRawData.Name, animationState.name);

        Animation animation = rawData.Animation;
        SkinnedMeshRenderer renderer = subRawData.Renderer;

        animation.Play(animationState.name);
        float sampleTime = 0f;
        for (int i = 0; i < frameCount; i++)
        {
            animationState.time = sampleTime;

            animation.Sample();
            renderer.BakeMesh(tempMesh);
            for (var j = 0; j < tempMesh.vertexCount; j++)
            {
                var vertex = tempMesh.vertices[j];
                animationMap.SetPixel(j, i, new Color(vertex.x, vertex.y, vertex.z));
            }

            sampleTime += timePerFrame;
        }
        animationMap.Apply();

        BakedData bakedData = new BakedData(animationMap.name, animationState.length, animationMap);
        bakedDataList.Add(bakedData);
    }
}
