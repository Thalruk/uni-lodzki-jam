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
        lr.positionCount = 2;

        lr.startWidth = thinWidth;
        lr.endWidth = thinWidth;
        lr.startColor = new Color(1, 0, 0, 0.4f);
        lr.endColor = new Color(1, 0, 0, 0.4f);

        while (timer < telegraphTime)
        {
            Vector3 currentDirection = transform.right;

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + currentDirection * 30f);

            timer += Time.deltaTime;
            yield return null;
        }

        lr.startWidth = thickWidth;
        lr.endWidth = thinWidth;
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        Vector3 finalShotDir = transform.right;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, thickWidth / 2f, finalShotDir, 30f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            Debug.Log("Laser trafi³ gracza!");
        }

        yield return new WaitForSeconds(laserDuration);
        Destroy(gameObject);
    }
}