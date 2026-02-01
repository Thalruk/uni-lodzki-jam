using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour, IDamaglable
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
    bool bited = false;
    float bitedCooldown = 0f;
    HealthSystem healthSystem;

    [SerializeField] Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    HealthSystem playerHealthSystem;



    private void OnEnable() => fow.OnPlayerSeenChanged += HandleDetection;
    private void OnDisable() => fow.OnPlayerSeenChanged -= HandleDetection;
    private void Awake()
    {
        patrolPoints.Clear();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        patrolPoints.AddRange(patrolPointHolder.GetComponentsInChildren<PatrolPoint>());

        if (patrolPoints.Count < 2)
        {
            Debug.LogError($"{name} doesnt have enough patrol points!");
        }
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetStartingHealth(1);
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

    // Patrol pause
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
        print("REseting");
        caughtByPlayer = false;
    }


    // Chase and patroling
    private void FixedUpdate()
    {

        Vector2 targetPos;

        if (isChasing && fow.player != null && !bited)
        {
            targetPos = fow.player.transform.position;
            if (caughtByPlayer)
            {
                rb2D.velocity = Vector2.zero;
                return;
            }

            // Attack
            float dist = Vector2.Distance(transform.position, targetPos);
            if (dist < 7f)
            {
                spriteRenderer.sprite = sprites[1];
            }
            if (dist < 2.5f)
            {
                spriteRenderer.sprite = sprites[0];
                playerHealthSystem = fow.player.gameObject.GetComponent<HealthSystem>();
                playerHealthSystem.ChangeHealth(-1);
                rb2D.velocity = Vector2.zero;
                bited = true;
                bitedCooldown = 1f;
                return;
            }
        }
        else
        {
            if (bited)
            {
                bitedCooldown -= Time.deltaTime;
                rb2D.velocity = Vector2.zero;
                if (bitedCooldown < 0)
                    bited = false;
            }
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
        transform.GetChild(transform.childCount - 1).GetComponent<Transform>().position = transform.position;
        transform.GetChild(transform.childCount - 1).GetComponent<Transform>().transform.localRotation = transform.rotation;
        //caughtByPlayer = false;
    }

    public void TakeDamage()
    {
        healthSystem.ChangeHealth(-1);
    }
}
