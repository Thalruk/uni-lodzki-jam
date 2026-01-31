using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle = 0;
    public PlayerMovement player;

    public bool playerInView = false;

    public CircleCollider2D circleCollider;
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = radius;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovement>();
        }
    }
    private void Update()
    {
        Vector2 viewAngleA = DirFromAngle(-angle / 2);
        Vector2 viewAngleB = DirFromAngle(angle / 2);
        if (player)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            //Debug.Log($"LEFT {viewAngleA.normalized}");
            //Debug.Log($"DIR {direction}");
            //Debug.Log($"RIGHT {viewAngleB.normalized}");
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 10, Color.red);
            Debug.Log($"dir-a {Vector2.Angle(direction, viewAngleA)}");
            Debug.Log($"dir-a {Vector2.Angle(direction, viewAngleB)}");
            if (Vector2.Angle(direction, viewAngleA) <= angle && Vector2.Angle(direction, viewAngleB) <= angle)
            {
                playerInView = true;
            }
            else
            {
                playerInView = false;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
            playerInView = false;
        }
    }
}