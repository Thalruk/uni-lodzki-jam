using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowPart : ItemBaseClass
{
    [SerializeField] TextMeshProUGUI partsUI;
    protected override void Collect()
    {
        if (BowCollector.bowParts <= 1 ||!PlayerMovement.isItemsFull)
        {
            print(PlayerMovement.isItemsFull);
            BowCollector.CollectPart();
            partsUI.text = BowCollector.bowParts.ToString() + "/ 3";
            Destroy(gameObject);
        }

    }
}
