using System.Collections;
using UnityEngine;

public class BeholderLaser : MonoBehaviour
{
    private LineRenderer lr;
    private Transform target;

    [Header("Laser Timing")]
    [SerializeField] float telegraphTime = 2f;
    [SerializeField] float laserDuration = 0.8f;

    [Header("Visuals")]
    [SerializeField] float thinWidth = 0.05f;
    [SerializeField] float thickWidth = 1.2f;
    [SerializeField] float maxDistance = 30f;

    [Header("Combat Settings")]
    [SerializeField] int damage = 25;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float damageTickRate = 0.2f;

    void InitRenderer()
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
    }

    public void Setup(Transform player)
    {
        target = player;
        InitRenderer();
    }

    void Start()
    {
        InitRenderer();
        if (lr != null) StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        float timer = 0;
        lr.positionCount = 2;

        while (timer < telegraphTime)
        {
            UpdateLaserBeam(thinWidth, new Color(1, 0, 0, 0.4f));
            timer += Time.deltaTime;
            yield return null;
        }

        float shotTimer = 0;
        float nextDamageTime = 0;

        while (shotTimer < laserDuration)
        {
            float currentDist = UpdateLaserBeam(thickWidth, Color.red, true);

            if (Time.time >= nextDamageTime)
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, thickWidth / 2f, transform.right, currentDist, playerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out HealthSystem health))
                    {
                        health.ChangeHealth(-1);
                        nextDamageTime = Time.time + damageTickRate;
                    }
                }
            }

            shotTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    float UpdateLaserBeam(float width, Color color, bool isSpike = false)
    {
        lr.startWidth = width;
        lr.endWidth = isSpike ? 0.05f : width;
        lr.startColor = color;
        lr.endColor = color;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxDistance, obstacleMask);

        float distance = hit.collider != null ? hit.distance : maxDistance;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.right * distance);

        return distance;
    }
}