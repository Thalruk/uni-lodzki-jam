using System.Collections.Generic;
using UnityEngine;

public class Beholder : MonoBehaviour
{
    private List<BeholderMiniEye> allEyes = new List<BeholderMiniEye>();
    [SerializeField] float criticalDistance = 5f;

    [Header("Phase 2 Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float laserCooldown = 5f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject openEye;
    [SerializeField] GameObject closedEye;

    private bool isPhase2 = false;
    private float laserTimer;
    private Transform playerTransform;

    [Header("Main Eye Visuals")]
    [SerializeField] Transform mainEyePupil;
    [SerializeField] float pupilOffset = 0.5f;
    [SerializeField] float rotationSpeed = 60f;

    private float currentMainEyeAngle;

    public void RegisterEyes(List<BeholderMiniEye> eyes)
    {
        allEyes = eyes;
        foreach (var eye in allEyes)
        {
            var fow = eye.GetComponent<EnemyFieldOfView>();
            fow.OnPlayerSeenChanged += (spotted) => OnEyeStatusChanged(eye, spotted);
        }
    }

    private void OnEyeStatusChanged(BeholderMiniEye reporter, bool spotted)
    {
        if (reporter.isDead || reporter.fow.player == null) return;

        playerTransform = reporter.fow.player.transform;

        if (!isPhase2)
        {
            HandlePhase1Combat(reporter, spotted);
        }
    }

    private void Update()
    {
        allEyes.RemoveAll(eye => eye == null);

        if (!isPhase2)
        {
            CheckForPhaseTransition();
            HandlePhase1Update();
        }
        else
        {
            HandlePhase2();
        }
    }

    private void CheckForPhaseTransition()
    {
        if (allEyes.Count > 0 && allEyes.TrueForAll(e => e.isDead))
        {
            StartPhase2();
        }
    }

    void HandlePhase1Update()
    {
        BeholderMiniEye eyeWithPlayer = allEyes.Find(e => !e.isDead && e.fow.playerInView && e.fow.player != null);

        if (eyeWithPlayer != null)
        {
            float dist = Vector2.Distance(transform.position, eyeWithPlayer.fow.player.transform.position);

            if (dist <= criticalDistance)
            {
                foreach (var eye in allEyes)
                {
                    if (eye != null && !eye.isDead)
                    {
                        eye.forceLookTarget = eyeWithPlayer.fow.player.transform;
                        eye.ShootIfReady(eyeWithPlayer.fow.player.transform);
                    }
                }
            }
            else
            {
                ResetForceLook();
            }
        }
        else
        {
            ResetForceLook();
        }
    }

    void HandlePhase1Combat(BeholderMiniEye reporter, bool spotted)
    {
        float dist = Vector2.Distance(transform.position, reporter.fow.player.transform.position);

        if (dist <= criticalDistance)
        {
            foreach (var eye in allEyes)
            {
                if (eye != null && !eye.isDead)
                {
                    eye.forceLookTarget = reporter.fow.player.transform;
                    eye.ShootIfReady(reporter.fow.player.transform);
                }
            }
        }
        else
        {
            ResetForceLook();
            if (spotted) reporter.ShootIfReady();
        }
    }

    void HandlePhase2()
    {
        if (playerTransform == null) return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

        Vector2 dirToPlayer = (playerTransform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        currentMainEyeAngle = Mathf.MoveTowardsAngle(currentMainEyeAngle, targetAngle, rotationSpeed * Time.deltaTime);

        if (mainEyePupil != null)
        {
            mainEyePupil.rotation = Quaternion.Euler(0, 0, currentMainEyeAngle);

            Vector2 visualOffset = new Vector2(Mathf.Cos(currentMainEyeAngle * Mathf.Deg2Rad), Mathf.Sin(currentMainEyeAngle * Mathf.Deg2Rad)) * pupilOffset;
            mainEyePupil.localPosition = visualOffset;
        }

        foreach (var eye in allEyes)
        {
            if (eye != null)
            {
                eye.forceLookTarget = playerTransform;
                eye.ShootIfReady(playerTransform);
            }
        }

        laserTimer -= Time.deltaTime;
        if (laserTimer <= 0)
        {
            FireBigLaser();
            laserTimer = laserCooldown;
        }
    }

    void FireBigLaser()
    {
        if (laserPrefab != null && mainEyePupil != null)
        {
            GameObject laser = Instantiate(laserPrefab, mainEyePupil.position, mainEyePupil.rotation, mainEyePupil);

            if (laser.TryGetComponent<BeholderLaser>(out var laserScript))
            {
                laserScript.Setup(playerTransform);
            }
        }
    }
    void StartPhase2()
    {
        isPhase2 = true;
        laserTimer = 2f;
        Debug.Log("BEHOLDER PHASE 2");
        openEye.SetActive(true);
        closedEye.SetActive(false);

        foreach (var eye in allEyes)
        {
            if (eye != null)
            {
                eye.Revive();
                eye.SetFireRate(3f);
            }
        }
    }

    void ResetForceLook()
    {
        foreach (var eye in allEyes)
        {
            if (eye != null) eye.forceLookTarget = null;
        }
    }
}