using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMask : ItemBaseClass
{
    public static event Action OnFoxMaskUsed;
    private void Start()
    {
        isPassive = true;
        cooldown = 0;
    }
    protected override void Use()
    {
        OnFoxMaskUsed?.Invoke();
        
    }
    protected override void Collect()
    {
        base.Collect();
    }
    private void OnEnable()
    {
        Use();
    }
}
