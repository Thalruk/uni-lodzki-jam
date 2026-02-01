using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowPart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI partsUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (BowCollector.bowParts < 2 || !PlayerMovement.isItemsFull)
            {
                BowCollector.CollectPart();
                partsUI.text = BowCollector.bowParts.ToString() + "/ 3";
                Destroy(gameObject);
            }
            else if (PlayerMovement.isItemsFull && BowCollector.bowParts > 2)
            {
                collision.GetComponent<PlayerMovement>().ShowWarningTXT();
            }
        }
    }
}
