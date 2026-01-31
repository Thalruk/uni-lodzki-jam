using UnityEngine;

[ExecuteInEditMode]
public class FovConeVisualization : MonoBehaviour
{

    [SerializeField] Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_SourceTex", source);
        Graphics.Blit(source, destination, material, 0);
    }

}
