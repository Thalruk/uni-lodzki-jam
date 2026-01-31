using System.Collections.Generic;
using UnityEngine;

public class Beholder : MonoBehaviour
{
    private List<BeholderMiniEye> allEyes = new List<BeholderMiniEye>();
    [SerializeField] float criticalDistance = 4f;
    private bool isPhase2 = false;

    [SerializeField] Sprite openEye;
    public void RegisterEyes(List<BeholderMiniEye> eyes)
    {
        allEyes = eyes;
        foreach (var eye in allEyes)
        {
            var fow = eye.GetComponent<EnemyFieldOfView>();
            fow.OnPlayerSeenChanged += (seen) => OnEyeStatusChanged(eye, seen);
        }
    }

    private void OnEyeStatusChanged(BeholderMiniEye reporter, bool spotted)
    {
        if (reporter.isDead || reporter.fow.player == null) return;

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

    private void Update()
    {
        if (isPhase2)
        {
            return;
        }

        allEyes.RemoveAll(eye => eye == null);

        bool allDead = allEyes.Count > 0 && allEyes.TrueForAll(e => e.isDead);

        if (allDead)
        {
            StartPhase2();
            return;
        }

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

    private void ResetForceLook()
    {
        foreach (var eye in allEyes)
        {
            if (eye != null) eye.forceLookTarget = null;
        }
    }

    void StartPhase2()
    {
        isPhase2 = true;
        Debug.Log("--- PHASE 2: WSZYSTKIE OCZY SIÊ OTWIERAJ¥ ---");

        foreach (var eye in allEyes)
        {
            if (eye != null)
            {
                eye.Revive();
            }
        }

        GetComponent<SpriteRenderer>().sprite = openEye;
    }
}