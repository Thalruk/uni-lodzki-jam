using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class BowCollector 
{
    public static int bowParts;
    public static bool isBowCompleted = false;

    public static void CollectPart()
    {
        bowParts++;
        if(bowParts == 3)
        {
            isBowCompleted = true;
            GameObject.FindAnyObjectByType<BowHandler>().CollectBowl();
        }else if (PlayerMovement.isItemsFull)
        {
            GameObject.FindAnyObjectByType<BowHandler>().CollectBowl();
        }
    }
}
