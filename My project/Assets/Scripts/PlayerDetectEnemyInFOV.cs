using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectEnemyInFOV : MonoBehaviour
{
    public List<BoxCollider2D> enemies = new List<BoxCollider2D>();
    [SerializeField] LayerMask enemyLayer;

    Vector2 playerViewDir;
    CreateFOVCone playerCone;
    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = transform.parent.GetComponentInChildren<CreateFOVCone>().viewRange;
        playerCone = transform.parent.GetComponentInChildren<CreateFOVCone>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            print("AddingEnemy");
            BoxCollider2D enemyCollider = collision.GetComponent<BoxCollider2D>();
            if (enemyCollider != null && !enemies.Contains(enemyCollider))
            {
                enemies.Add(enemyCollider);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BoxCollider2D enemyCollider = collision.GetComponent<BoxCollider2D>();
            if (enemyCollider != null && enemies.Contains(enemyCollider))
            {
                enemies.Remove(enemyCollider);
            }
        }
    }

    private void LateUpdate()
    {
        playerViewDir = -transform.up;
        foreach (BoxCollider2D enemy in enemies)
        {
            Vector2 point = enemy.bounds.ClosestPoint(transform.position);
            //for (int i = 0; i < points.Length; i++)
            //{
            Vector2 toEnemy = (point - (Vector2)transform.position).normalized;
            float dist = playerCone.viewRange;
            //if (dist > playerCone.viewRange)
            //    continue;

            float angleToPoint = Vector2.SignedAngle(playerViewDir, toEnemy);
            if (Mathf.Abs(angleToPoint) > playerCone.playerFOV * 0.5)
                continue;

            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, toEnemy, dist, enemyLayer);
            Debug.DrawLine(transform.position, hit2D.point, UnityEngine.Color.green);
            if (hit2D.collider != null && hit2D.collider.CompareTag("Enemy"))
            {
                EnemyController enemyController = hit2D.collider.gameObject.GetComponent<EnemyController>();
                if (!enemyController.caughtByPlayer)
                {
                    enemyController.caughtByPlayer = true;
                    break;
                }
            }

            //}
        }
    }
}
