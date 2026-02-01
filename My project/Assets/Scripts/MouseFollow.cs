using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] float offset = 20f;
    [SerializeField] float deadzone = 5f;
    [SerializeField] float smoothSpeed = 10f;
    [SerializeField] Transform eye;

    private Vector3 targetLocalPos;
    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 myPos = transform.position;

        Vector2 diff = mousePos - myPos;
        float distance = diff.magnitude;

        if (distance < deadzone)
        {
            targetLocalPos = Vector3.zero;
        }
        else
        {
            float currentMove = Mathf.Min(distance, offset);
            targetLocalPos = diff.normalized * currentMove;
        }

        Vector3 finalPos = transform.position + targetLocalPos;
        eye.position = Vector3.Lerp(eye.position, finalPos, Time.deltaTime * smoothSpeed);
    }
}