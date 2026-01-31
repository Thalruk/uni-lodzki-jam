using UnityEngine;

//[ExecuteInEditMode]
public class ZaWarudo : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] float time, timeMul;

    private void Start()
    {
        this.enabled = false;
    }
    private void OnEnable()
    {
        time = 0f;
        timeMul = 1.5f;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        time += Time.unscaledDeltaTime * timeMul;
        time = Mathf.Clamp(time, 0f, 1.5f);
        material.SetTexture("_SourceTex", source);
        material.SetFloat("_TimeElapsed", time);
        Graphics.Blit(source, destination, material);
    }

    public void ResetTime()
    {
        timeMul = -timeMul;
    }
}
