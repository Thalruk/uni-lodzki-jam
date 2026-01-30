using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHandler : ItemBaseClass
{
    protected override void Collect()
    {
        if(BowCollector.bowParts == 3)
        {
            base.Collect();
        }
        else
        {
            BowCollector.bowParts++;
        }
    }
    protected override void Use()
    {
         
    }
}
