using UnityEngine;

public class TransparentNearPlayer : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float visibility = 0.5f;
    MaterialPropertyBlock mpb;
    Renderer r;
    private void Start()
    {
        r = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        r.GetPropertyBlock(mpb);
        GetComponent<SpriteRenderer>().material = mat;
        mpb.SetFloat("_Visibility", visibility);
        r.SetPropertyBlock(mpb);

    }
}
