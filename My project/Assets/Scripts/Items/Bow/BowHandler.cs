using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BowHandler : ItemBaseClass
{
    [SerializeField] GameObject arrow;
    private void Start()
    {
        transform.position = trashPoint.position;
        cooldown = 0.6f;
        isPassive = false;
        isHasDurationTime = false;
    }
    public void CollectBowl()
    {
        base.Collect(); 
    }
    protected override void Use()
    {
        Instantiate(arrow, transform.position,transform.rotation);
    }
}
