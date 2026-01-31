using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static event Action OnHealthChanged, OnDie;
    int health;
    public void SetStartingHealth(int value)
    {
        health = value;
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
