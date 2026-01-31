using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendigoMask : ItemBaseClass
{
    public static event Action OnVendigoMaskUsed, OnVendigoEndEffect;
    private void Start()
    {
        isPassive = true;
        isHasDurationTime = true;
        useDuration = 8f;
        cooldown = useDuration + cooldown;
    }
    protected override void Use()
    {
        OnVendigoMaskUsed?.Invoke();
    }
    protected override void Collect()
    {
        base.Collect();
    }
    protected override void Unequip()
    {
        OnVendigoEndEffect?.Invoke();
        base.Unequip();
    }
    private void OnDisable()
    {
        OnVendigoEndEffect?.Invoke();
    }
    private void OnDestroy()
    {
        OnVendigoEndEffect?.Invoke();
    }
}
