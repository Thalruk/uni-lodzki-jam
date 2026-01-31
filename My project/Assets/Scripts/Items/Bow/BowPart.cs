using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowPart : ItemBaseClass
{
    [SerializeField] TextMeshProUGUI partsUI;
    protected override void Collect()
    {
      BowCollector.bowParts++;
      partsUI.text = BowCollector.bowParts.ToString();
      Destroy(gameObject);
    }
}
