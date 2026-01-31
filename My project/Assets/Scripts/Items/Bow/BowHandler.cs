using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BowHandler : ItemBaseClass
{

    private void Start()
    {
        transform.position = trashPoint.position;
    }
    public void CollectBowl()
    {
        base.Collect();
        
    }
    protected override void Use()
    {
        print("shoot");
    }
}
