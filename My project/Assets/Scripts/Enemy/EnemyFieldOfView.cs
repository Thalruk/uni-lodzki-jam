using System;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle = 0;
    public PlayerMovement player;

    public bool playerInView = false;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] CircleCollider2D circleCollider;

    public event Action<bool> OnPlayerSeenChanged;
    private bool lastDetectedState = false;

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.z;
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        if (transform.localScale.x > 1)
        {
            circleCollider.radius = radius * transform.localScale.x;
        }
        else
        {
            circleCollider.radius = radius / transform.localScale.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovement>();
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player == null)
                player = collision.GetComponent<PlayerMovement>();
            EnemyController enemyController = transform.parent.GetComponent<EnemyController>();
            Vector2 dirFromPlayerView = (-player.transform.up).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            Vector2 dirFromPlayer = (transform.position - player.transform.position).normalized;

            float angleToPlayer = Vector2.SignedAngle(dirFromPlayerView, dirFromPlayer);
            CreateFOVCone fovCone = player.transform.GetComponentInChildren<CreateFOVCone>();
            print(angleToPlayer);
            if (!enemyController.caughtByPlayer &&
                Mathf.Abs(angleToPlayer) <= fovCone.playerFOV * 0.5f &&
                distanceToPlayer <= fovCone.viewRange)
            {
                print("Enemy freez");
                enemyController.caughtByPlayer = true;
            }
            else if (enemyController.caughtByPlayer)
            {
                print("Enemy unfreez");
                enemyController.caughtByPlayer = false;
            }
        }
    }
    */
    private void Update()
    {
        playerInView = false;

        if (player)
        {
            Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            float angleToPlayer = Vector2.Angle(transform.right, dirToPlayer);

            if (angleToPlayer < angle / 2f)
            {
                Debug.DrawLine(transform.position, player.transform.position, Color.blue);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask);

                if (hit.collider == null)
                {
                    playerInView = true;
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    playerInView = true;
                }
            }
        }
        if (playerInView != lastDetectedState)
        {
            lastDetectedState = playerInView;
            OnPlayerSeenChanged?.Invoke(playerInView);
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
    private void OnValidate()
    {
        if (circleCollider != null) circleCollider.radius = radius / transform.localScale.x;
    }
    private void OnDrawGizmos()
    {
        if (transform == null) return;

        Gizmos.color = Color.red;

        Vector3 viewAngleA = DirFromAngle(-angle / 2);
        Vector3 viewAngleB = DirFromAngle(angle / 2);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * radius);
    }
}