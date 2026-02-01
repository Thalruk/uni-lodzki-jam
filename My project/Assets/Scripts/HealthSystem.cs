using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static event Action OnHealthChanged, OnDie;
    int health;
    [SerializeField] GameObject grid;
    [SerializeField] Image healthImage;
    public void SetStartingHealth(int value)
    {
        health = value;
        if(grid != null && healthImage)
        {
            for(int i = 0; i < health; i++)
            {
                Instantiate(healthImage, grid.transform);
            }

        }
    }
    void UpdateHealth(int value)
    {
        if(grid==null)return;
        switch (Mathf.Sign(value))
        {
            case 1:
                Instantiate(healthImage, grid.transform);
                break;
            case -1:
                Destroy(grid.transform.GetChild(0));
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
