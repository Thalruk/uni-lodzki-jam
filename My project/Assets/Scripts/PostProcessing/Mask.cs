using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Mask : MonoBehaviour
{
    RenderTexture maskRT;
    CommandBuffer cb;
    [SerializeField] Material defaultMaskMaterial;
    [SerializeField] List<Material> maskMaterial = new List<Material>();
    [SerializeField] List<GameObject> coneGameObj = new List<GameObject>();
    Renderer cone;
    void Start()
    {
        maskRT = new RenderTexture(
            Screen.width,
            Screen.height,
            0,
            RenderTextureFormat.R8 // idealne dla maski
        );

        maskRT.wrapMode = TextureWrapMode.Clamp;
        maskRT.filterMode = FilterMode.Bilinear;
        maskRT.Create();

        cb = new CommandBuffer();
        cb.name = "Screen Mask Pass";

        cb.SetRenderTarget(maskRT);
        cb.ClearRenderTarget(false, true, Color.black);

        int i = 0;
        foreach (GameObject go in coneGameObj)
        {
            Renderer r = go.GetComponent<Renderer>();
            cb.DrawRenderer(r, maskMaterial[i], 0, 0);
            i++;
        }
        //cone = coneGameObj.GetComponent<Renderer>();
        //cb.DrawRenderer(cone, maskMaterial, 0, 0);

        Camera.main.AddCommandBuffer(
            CameraEvent.AfterForwardOpaque,
            cb
        );

        Shader.SetGlobalTexture("_ScreenMaskTexture", maskRT);
    }

    public void AddLightObj(GameObject go, Material mat)
    {
        coneGameObj.Add(go);
        if (!mat)
            maskMaterial.Add(defaultMaskMaterial);
        else
            maskMaterial.Add(mat);
    }


}
