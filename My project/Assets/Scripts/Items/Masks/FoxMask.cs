using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMask : ItemBaseClass
{
    public static event Action OnFoxMaskUsed;

    protected override void Use()
    {
        OnFoxMaskUsed?.Invoke();
    }
    protected override void Collect()
    {
        base.Collect();
    }

}
