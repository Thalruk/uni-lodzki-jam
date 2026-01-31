using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class SpiritMask : ItemBaseClass
{
    GameObject camera;
    private void Start()
    {
        isPassive = true;
        isHasDurationTime = true;
        useDuration = 8f;
        cooldown = useDuration + cooldown;
        camera = FindAnyObjectByType<Camera>().gameObject;
        camera.GetComponent<PostProcessVolume>().enabled = false;
    }
    protected override void Use()
    {
        camera.GetComponent<PostProcessVolume>().enabled = true;
        Time.timeScale = 0f;
        camera.GetComponent<ZaWarudo>().enabled = true;
    }
    protected override void Unequip()
    {
        camera.GetComponent<ZaWarudo>().ResetTime();
        Invoker.InvokeDelayed(UnequipRest, 1f);
    }

    private void UnequipRest()
    {
        camera.GetComponent<PostProcessVolume>().enabled = false;
        Time.timeScale = 1f;
        base.Unequip();
        camera.GetComponent<ZaWarudo>().enabled = false;
    }
}
