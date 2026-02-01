using UnityEngine;

public class ChangeAmbientVal : MonoBehaviour
{
    Camera cam;
    [SerializeField] Material material;
    float ambient;
    float maxDist;
    private void Start()
    {
        ambient = 0.1f;
        maxDist = GetComponent<CircleCollider2D>().radius;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float dist = Vector2.Distance(transform.position, collision.transform.position);
            dist /= maxDist;
            ambient = Mathf.Lerp(0.6f, 0.1f, dist);
            material.SetFloat("_Ambient", ambient);
        }
    }
}
