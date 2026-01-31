using UnityEngine;

public class HideInTheShadows : MonoBehaviour
{
    [SerializeField] Material mat;
    private void Start()
    {
        GetComponent<SpriteRenderer>().material = mat;
    }
}
