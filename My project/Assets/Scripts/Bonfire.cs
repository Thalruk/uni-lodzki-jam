using UnityEngine;

public class Bonfire : MonoBehaviour
{
    Camera cam;
    [SerializeField] Material material;
    GameObject fire;
    Mask mask;

    float t;
    Vector3 baseScale;
    private void Start()
    {
        cam = Camera.main;
        mask = cam.GetComponent<Mask>();
        fire = transform.GetChild(0).gameObject;
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

        fire.transform.localScale = baseScale + Vector3.one * scale * 5f;
    }
}
