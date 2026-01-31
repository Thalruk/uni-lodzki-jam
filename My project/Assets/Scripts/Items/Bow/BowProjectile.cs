using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowProjectile : MonoBehaviour
{
    [SerializeField] float arrowSpeed;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(-transform.up*arrowSpeed, ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        if(Mathf.Abs(rb.velocity.y)<=0.2 && Mathf.Abs(rb.velocity.x) <= 0.2)
        {
            Destroy(gameObject);
        }
    }
}
