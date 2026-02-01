using UnityEngine;

public class LightMask : MonoBehaviour
{
    Camera cam;
    [SerializeField] Material material;
    GameObject fire;
    Mask mask;

    float t;
    Vector3 baseScale;
    private void Awake()
    {
        cam = Camera.main;
        mask = cam.GetComponent<Mask>();
        fire = gameObject;
        fire.GetComponent<Renderer>().material = material;

        mask.AddLightObj(fire, material);

        t = transform.position.x * 452897 % 2574;
        baseScale = fire.transform.localScale;
    }

    private void Update()
    {
        t += Time.deltaTime;
        float scale = Mathf.Sin(t);
        scale += Mathf.Sin(t * 2 + 145) * 0.5f;
        scale += Mathf.Sin(t * 3 + 984) * 0.3f;

        fire.transform.localScale = Vector3.Max(baseScale + Vector3.one * scale * 5f, baseScale);
    }

    private void OnDestroy()
    {
        mask.RemoveLightObj(fire);
    }
}
