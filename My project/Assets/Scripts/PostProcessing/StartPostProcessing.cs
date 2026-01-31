using UnityEngine;

[ExecuteInEditMode]
public class StartPostProcessing : MonoBehaviour
{

    [SerializeField] Material material;
    RenderTexture currentSource, currentDest;

    private void Start()
    {

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        currentSource = RenderTexture.GetTemporary(source.width, source.height);
        currentDest = RenderTexture.GetTemporary(source.width, source.height);

        material.SetTexture("_SourceTex", source);
        Graphics.Blit(source, currentDest, material, 1);
        currentSource = currentDest;
        material.SetTexture("_SourceTex", currentSource);
        Graphics.Blit(currentSource, destination, material, 0);

        RenderTexture.ReleaseTemporary(currentSource);
        RenderTexture.ReleaseTemporary(currentDest);
    }
}
