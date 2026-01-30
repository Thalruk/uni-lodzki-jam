using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform patrolPoint;
    [SerializeField] bool movingTowardPatrol = true;
    Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        startPoint = transform;
    }

    private void Update()
    {
        if (movingTowardPatrol)
        {
            rb2D.MovePosition(patrolPoint.position);
        }
        else
        {
            rb2D.MovePosition(startPoint.position);
        }
    }

}
