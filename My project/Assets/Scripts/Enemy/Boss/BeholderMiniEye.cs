using System.Collections.Generic;
using UnityEngine;

public class BeholderMiniEye : MonoBehaviour, IDamaglable
{
    [SerializeField] public EnemyFieldOfView fow;
    [SerializeField] public Vector2 lookDir;
    [SerializeField] float offset = 0.2f;
    [SerializeField] Transform eyeVisual;
    [SerializeField] Sprite openEye;
    [SerializeField] Sprite closedEye;
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] AudioSource laserSource;
    [SerializeField] AudioSource hitSource;

    [Header("Patrol & Smoothness")]
    [SerializeField] float patrolAngle = 30f;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float rotationSpeed = 360f;

    [Header("Combat")]
    [SerializeField] List<Projectile> projectiles;
    [SerializeField] float fireRate = 1.5f;
    [SerializeField] float shootOffset = 1.5f;
    private float fireTimer;

    private float baseAngle;
    private float currentAngle;
    private float randomOffset;

    public bool isDead = false;

    void Awake()
    {
        fow = GetComponentInChildren<EnemyFieldOfView>();
        randomOffset = Random.Range(0f, 100f);
    }

    void Start()
    {
        baseAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        currentAngle = baseAngle;
        healthSystem.SetStartingHealth(1);
        fireTimer = Random.Range(0f, fireRate);
    }

    void Update()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (fow.playerInView && fow.player != null)
        {
            ShootIfReady(fow.player.transform);
        }
    }

    public void ShootIfReady(Transform target = null)
    {
        if (isDead) return;


        if (fireTimer <= 0)
        {
            float randomDelay = Random.Range(0f, 0.2f);

            forceLookTarget = target;

            Invoke(nameof(ExecuteShot), randomDelay);
            print("SHOOT");

            fireTimer = fireRate + randomDelay;
        }
    }

    private void ExecuteShot()
    {
        if (isDead) return;

        Shoot(forceLookTarget);
    }

    void Shoot(Transform target = null)
    {
        if (projectiles.Count == 0) return;

        Projectile bulletPrefab = projectiles[Random.Range(0, projectiles.Count)];
        Projectile bullet = Instantiate(bulletPrefab.gameObject, new Vector3(eyeVisual.position.x, eyeVisual.position.y, -0.1f), transform.rotation).GetComponent<Projectile>();

        Vector2 shootDirection;

        if (target != null)
        {
            Vector3 targetPos = target.position;
            Vector3 randomSpread = (Vector3)Random.insideUnitCircle * shootOffset;
            Vector3 finalTarget = targetPos + randomSpread;
            shootDirection = (finalTarget - eyeVisual.position).normalized;
        }
        else
        {
            shootDirection = transform.right;
        }

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = shootDirection * bullet.speed;
        }

        if (TryGetComponent<AudioSource>(out var source))
        {
            laserSource.pitch = Random.Range(0.85f, 1.15f);
            laserSource.Play();
        }
    }
    public void SetFireRate(float newRate) => fireRate = newRate;

    public Transform forceLookTarget;

    void LateUpdate()
    {
        float targetAngle;

        if (forceLookTarget != null)
        {
            Vector2 dirToTarget = (forceLookTarget.position - transform.position).normalized;
            targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            lookDir = dirToTarget;
        }
        else if (fow != null && fow.playerInView && fow.player != null)
        {
            Vector2 dirToPlayer = (fow.player.transform.position - transform.position).normalized;
            targetAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
            lookDir = dirToPlayer;
        }
        else
        {
            float wave = Mathf.Sin(Time.time * patrolSpeed + randomOffset) * patrolAngle;
            targetAngle = baseAngle + wave;
            float rad = targetAngle * Mathf.Deg2Rad;
            lookDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        if (eyeVisual != null)
        {
            eyeVisual.position = transform.position + (Vector3)lookDir * offset;
        }
    }

    public void Revive()
    {
        isDead = false;
        fireTimer = fireRate;
        GetComponent<SpriteRenderer>().sprite = openEye;
        if (eyeVisual != null) eyeVisual.gameObject.SetActive(true);
        if (fow != null) fow.enabled = true;
        if (TryGetComponent<LineRenderer>(out var lr))
        {
            lr.startColor = Color.white;
            lr.endColor = Color.white;
        }
    }

    public void TakeDamage()
    {
        if (TryGetComponent<AudioSource>(out var source))
        {
            hitSource.pitch = 1;
            hitSource.Play();
        }
        isDead = true;
        fireTimer = float.MaxValue;
        GetComponent<SpriteRenderer>().sprite = closedEye;
        eyeVisual.gameObject.SetActive(false);
        if (fow != null) fow.enabled = false;
        if (TryGetComponent<LineRenderer>(out var lr))
        {
            lr.startColor = Color.gray;
            lr.endColor = Color.black;
        }
    }
}