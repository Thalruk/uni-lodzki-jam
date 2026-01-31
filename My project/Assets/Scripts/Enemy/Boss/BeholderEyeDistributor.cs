using UnityEngine;

public class BeholderEyeDistributor : MonoBehaviour
{
    [SerializeField] GameObject eyePrefab;
    [SerializeField] Transform eyeHolder;
    [SerializeField] int eyeCount = 10;
    [SerializeField] float minRadius = 2.5f;
    [SerializeField] float maxRadius = 3.5f;
    [Range(0, 1)][SerializeField] float randomness = 0.3f;
    void Start()
    {
        DistributeEyes();
    }

    void DistributeEyes()
    {
        float angleStep = 360f / eyeCount;

        for (int i = 0; i < eyeCount; i++)
        {
            float currentAngle = (i * angleStep) + Random.Range(-angleStep * randomness, angleStep * randomness);

            float currentRadius = Random.Range(minRadius, maxRadius);

            Vector3 spawnPos = transform.position + new Vector3(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad),
                0
            ) * currentRadius;

            GameObject newEye = Instantiate(eyePrefab, spawnPos, Quaternion.identity, eyeHolder);
            Vector2 startLookDir = (newEye.transform.position - transform.position).normalized;
            newEye.GetComponent<BeholderMiniEye>().lookDir = startLookDir;

            if (newEye.TryGetComponent<TentacleLink>(out var link))
            {
                link.SetAnchor(transform);
            }
        }
    }
}