using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyImageEffect : MonoBehaviour
{
    public Shader screenShader;
    private Material screenMaterial;

    // Start is called before the first frame update
    void Start()
    {
        screenShader = Shader.Find("Hidden/MYImageEffectShader");
        screenMaterial = new Material(screenShader);
    }

    //屏幕特效事件函数
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, screenMaterial);
    }
}
