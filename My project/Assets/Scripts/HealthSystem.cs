using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static event Action OnHealthChanged, OnDie;
    int health;
    [SerializeField] GameObject grid;
    [SerializeField] Image healthImage;

    Camera cam;
    Vignette vignette;
    float vignetteIntensity = 0f;
    float currentIntensity = 0f;
    float t = 0f;
    private void Start()
    {
        cam = Camera.main;
        cam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out vignette);

    }

    private void Update()
    {
        if (!grid)
            return;
        currentIntensity = Mathf.Lerp(currentIntensity, vignetteIntensity, 0.1f);
        vignette.intensity.value = currentIntensity;
        print(vignetteIntensity);
    }
    public void SetStartingHealth(int value)
    {
        health = value;
        if (grid && healthImage)
        {
            for (int i = 0; i < health; i++)
            {
                Instantiate(healthImage, grid.transform);
            }

        }
    }
    void UpdateHealth(int value)
    {
        if (grid == null) return;
        switch (Mathf.Sign(value))
        {
            case 1:
                Instantiate(healthImage, grid.transform);
                vignetteIntensity -= 0.3f;
                print("DMG");
                break;
            case -1:
                Destroy(grid.transform.GetChild(0).gameObject);
                vignetteIntensity += 0.3f;
                print("DMG");
                break;
        }

    }
    public void ChangeHealth(int value)
    {
        health += value;
        OnHealthChanged?.Invoke();
        UpdateHealth(value);
        if (health <= 0)
        {
            OnDie?.Invoke();
        }
    }
}
