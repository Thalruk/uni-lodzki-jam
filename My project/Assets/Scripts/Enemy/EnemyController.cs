using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<PatrolPoint> patrolPoints;
    [SerializeField] Transform patrolPointHolder;
    private int actualPatrolPoint = 0;

    Rigidbody2D rb2D;
    [SerializeField] float baseSpeed;
    [Range(0.2f, 2)]
    [SerializeField] float enragedSpeedModifier;
    private float currentSpeed;
    [SerializeField] float baseRotateSpeed;
    [Range(0.2f, 2)]
    [SerializeField] float enragedRotationSpeedModifier;
    private float currentRotateSpeed;
    private float timer = 0;

    [SerializeField] EnemyFieldOfView fow;
    private bool isChasing = false;
    private bool isWaiting = false;
    public bool caughtByPlayer = false;

    private void OnEnable() => fow.OnPlayerSeenChanged += HandleDetection;
    private void OnDisable() => fow.OnPlayerSeenChanged -= HandleDetection;
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

    private void HandleDetection(bool spotted)
    {
        isChasing = spotted;
        if (isChasing)
        {
            currentSpeed = baseSpeed * enragedSpeedModifier;
            currentRotateSpeed = baseRotateSpeed * enragedRotationSpeedModifier;
            isWaiting = false;
        }
        else
        {
            currentSpeed = baseSpeed;
            currentRotateSpeed = baseRotateSpeed;
        }
    }
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, patrolPoints[actualPatrolPoint].transform.position);

        if (distance < patrolPoints[actualPatrolPoint].checkRadius && !isWaiting)
        {
            isWaiting = true;
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (timer >= patrolPoints[actualPatrolPoint].waitTime)
            {
                timer = 0;
                isWaiting = false;
                actualPatrolPoint = (actualPatrolPoint + 1) % patrolPoints.Count;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetPos;

        if (isChasing && fow.player != null)
        {
            targetPos = fow.player.transform.position;
            if (caughtByPlayer)
            {
                rb2D.velocity = Vector2.zero;
                return;
            }
        }
        else
        {
            if (isWaiting)
            {
                rb2D.velocity = Vector2.zero;
                return;
            }
            targetPos = patrolPoints[actualPatrolPoint].transform.position;
        }

        Vector2 direction = (targetPos - rb2D.position).normalized;
        rb2D.velocity = transform.right * baseSpeed;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), baseRotateSpeed * Time.fixedDeltaTime);
    }
}
