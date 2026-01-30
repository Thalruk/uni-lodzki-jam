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
            Debug.Log("close enough");
            if (patrolPoints[actualPatrolPoint].waitTime != 0)
            {
                Debug.Log("have to wait");

                if (timer >= patrolPoints[actualPatrolPoint].waitTime)
                {
                    actualPatrolPoint = (actualPatrolPoint + 1) % patrolPoints.Count;
                    timer = 0;
                    Debug.Log("finished waiting");

                }
                else
                {
                    timer += Time.deltaTime;
                    Debug.Log("waiting");

                }
            }
            else
            {
                actualPatrolPoint = (actualPatrolPoint + 1) % patrolPoints.Count;
                Debug.Log("no wait");

            }

        }
        Vector2 direction = (patrolPoints[actualPatrolPoint].transform.position - transform.position).normalized;
        rb2D.velocity = direction * speed;
    }

}
