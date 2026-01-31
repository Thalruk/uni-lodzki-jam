using UnityEngine;

public class TentacleLink : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private Transform bodyAnchor;

    [Header("Macka Settings")]
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float waveStrength = 0.5f;
    [SerializeField] private float pointsPerUnit = 5f;
    [SerializeField] private float waveFrequency = 1.5f;
    [Header("Organic Movement")]
    [SerializeField] float floatSpeed = 0.5f;
    [SerializeField] float floatMagnitude = 1f;
    private Vector3 startLocalPos;
    private float noiseOffset;

    void Start()
    {
        startLocalPos = transform.localPosition;
        noiseOffset = Random.Range(0f, 100f);
    }
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 3;
    }

    void Update()
    {
        float noiseX = Mathf.PerlinNoise(Time.time * floatSpeed, noiseOffset);
        float noiseY = Mathf.PerlinNoise(noiseOffset, Time.time * floatSpeed);

        Vector3 offset = new Vector3(noiseX - 0.5f, noiseY - 0.5f, 0) * floatMagnitude;

        transform.localPosition = startLocalPos + offset;
    }

    public void SetAnchor(Transform anchor)
    {
        bodyAnchor = anchor;
    }

    void LateUpdate()
    {
        if (bodyAnchor == null) return;

        Vector3 startPos = bodyAnchor.position;
        Vector3 endPos = transform.position;
        float distance = Vector3.Distance(startPos, endPos);

        int pointsCount = Mathf.Max(3, Mathf.CeilToInt(distance * pointsPerUnit));
        lr.positionCount = pointsCount;

        Vector3 dir = (endPos - startPos).normalized;
        Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0);

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i / (float)(pointsCount - 1);

            Vector3 pointPos = Vector3.Lerp(startPos, endPos, t);

            float wave = Mathf.Sin(Time.time * waveSpeed + t * waveFrequency * distance)
                         * waveStrength
                         * Mathf.Sin(t * Mathf.PI);

            pointPos += perpendicular * wave;

            lr.SetPosition(i, pointPos);
        }
    }
}