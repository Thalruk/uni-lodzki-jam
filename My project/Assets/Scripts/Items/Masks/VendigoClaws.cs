using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendigoClaws : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamaglable damaglable))
        {
            damaglable.TakeDamage();
        }
    }
}
