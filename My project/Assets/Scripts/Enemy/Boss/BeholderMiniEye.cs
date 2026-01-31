using UnityEngine;

public class BeholderMiniEye : MonoBehaviour
{
    [SerializeField] public Vector2 lookDir;
    [SerializeField] float offset;
    [SerializeField] Transform eye;
    private void LateUpdate()
    {
        eye.position = transform.position + (Vector3)lookDir * offset;
    }
}
