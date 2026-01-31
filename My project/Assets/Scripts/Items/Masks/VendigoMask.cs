using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendigoMask : ItemBaseClass
{
    public static event Action OnVendigoMaskUsed;
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
}
