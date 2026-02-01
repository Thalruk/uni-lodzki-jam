using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FoxMask : ItemBaseClass
{
    Camera cam;
    public static event Action OnFoxMaskUsed, OnFoxMaskEndEffect;
    ChromaticAberration chromaticAberration;
    private void Start()
    {
        isPassive = true;
        isHasDurationTime = true;
        useDuration = 6f;
        cooldown = useDuration + cooldown;
        cam = Camera.main;
        cam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out chromaticAberration);

    }
    protected override void Unequip()
    {
        chromaticAberration.enabled.value = false;
        OnFoxMaskEndEffect?.Invoke();
        base.Unequip();
    }
    protected override void Use()
    {
        chromaticAberration.enabled.value = true;
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
