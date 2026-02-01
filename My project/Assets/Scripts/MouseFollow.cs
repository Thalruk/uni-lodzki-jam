using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] Transform eye;


    private void Update()
    {
        Vector2 dir = Vector2.up;




        eye.position = transform.position + (Vector3)dir * offset;
    }
}


