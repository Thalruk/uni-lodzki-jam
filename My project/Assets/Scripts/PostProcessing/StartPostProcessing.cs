using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StartPostProcessing : MonoBehaviour
{

    [SerializeField] Material _material;

    private void Start()
    {
        
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _material.SetTexture("_SourceTex", source);
        Graphics.Blit(source, destination, _material, 0);
    }
}
