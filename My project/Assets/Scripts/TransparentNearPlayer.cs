using UnityEngine;

public class TransparentNearPlayer : MonoBehaviour
{
    [SerializeField] Material mat;
    private void Start()
    {
        GetComponent<SpriteRenderer>().material = mat;
    }
}
