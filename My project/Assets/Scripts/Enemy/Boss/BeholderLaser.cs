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
        if (lr != null)
        {
            StartCoroutine(LaserRoutine());
        }
        else
        {
            Debug.LogError("BeholderLaser: Brak LineRenderer na prefabie!");
        }
    }

    IEnumerator LaserRoutine()
    {
        float timer = 0;
        Vector3 lastTargetDir = Vector3.right;

        lr.positionCount = 2;
        lr.startWidth = thinWidth;
        lr.endWidth = thinWidth;

        lr.startColor = new Color(1, 0, 0, 0.4f);
        lr.endColor = new Color(1, 0, 0, 0.4f);

        while (timer < telegraphTime)
        {
            if (target != null)
            {
                lastTargetDir = (target.position - transform.position).normalized;
            }

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + lastTargetDir * 30f);

            timer += Time.deltaTime;
            yield return null;
        }

        lr.startWidth = thickWidth;
        lr.endWidth = thickWidth;
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, thickWidth / 2f, lastTargetDir, 30f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            Debug.Log("Laser trafi³ gracza!");
        }

        yield return new WaitForSeconds(laserDuration);

        lr.enabled = false;
        Destroy(gameObject);
    }
}