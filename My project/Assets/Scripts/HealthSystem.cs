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
    public void ChangeHealth(int value)
    {
        health += value;
        OnHealthChanged?.Invoke();
        if (health <= 0)
        {
            OnDie?.Invoke();
        }
    }
}
