using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMask : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().health.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}
