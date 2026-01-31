using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EyeMask : ItemBaseClass
{
    List<GameObject> objectsForTracking = new List<GameObject>();
    Transform target;
    private void Start()
    {
        isPassive = true;
        isHasDurationTime = true;
        useDuration = 6f;
        cooldown = useDuration + cooldown; 
    }

}
