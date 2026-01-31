using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<PatrolPoint> patrolPoints;
    [SerializeField] Transform patrolPointHolder;
    int actualPatrolPoint = 0;

    Rigidbody2D rb2D;
    [SerializeField] float speed;

    [SerializeField] float timer = 0;

    private void Awake()
    {
        patrolPoints.Clear();
        rb2D = GetComponent<Rigidbody2D>();

        patrolPoints.AddRange(patrolPointHolder.GetComponentsInChildren<PatrolPoint>());

        if (patrolPoints.Count < 2)
        {
            Debug.LogError($"{name} doesnt have enough patrol points!");
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, patrolPoints[actualPatrolPoint].transform.position) < patrolPoints[actualPatrolPoint].checkRadius)
        {
            if (patrolPoints[actualPatrolPoint].waitTime != 0)
            {
                if (timer >= patrolPoints[actualPatrolPoint].waitTime)
                {
                    actualPatrolPoint = (actualPatrolPoint + 1) % patrolPoints.Count;
                    timer = 0;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            else
            {
                actualPatrolPoint = (actualPatrolPoint + 1) % patrolPoints.Count;
            }
        }
        Vector2 direction = (patrolPoints[actualPatrolPoint].transform.position - transform.position).normalized;
        rb2D.SetRotation(Quaternion.Euler(direction));
        rb2D.velocity = direction * speed;
    }


}
