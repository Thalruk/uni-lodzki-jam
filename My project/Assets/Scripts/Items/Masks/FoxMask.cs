using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMask : ItemBaseClass
{
    public static event Action OnFoxMaskUsed, OnFoxMaskEndEffect;
    private void Start()
    {
        isPassive = true;
        isHasDurationTime = true;
        useDuration = 6f;
        cooldown = useDuration + 4f;
    }
    protected override void Unequip()
    {
        OnFoxMaskEndEffect?.Invoke();
        base.Unequip();
    }
    protected override void Use()
    {
        OnFoxMaskUsed?.Invoke();
        
    }
    protected override void Collect()
    {
        base.Collect();
    }
    private void OnDisable()
    {
        OnFoxMaskEndEffect?.Invoke();
    }
    private void OnDestroy()
    {
        OnFoxMaskEndEffect?.Invoke();
    }
}
