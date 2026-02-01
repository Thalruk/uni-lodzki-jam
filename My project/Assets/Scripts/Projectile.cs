using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int speed;

    private void Awake()
    {
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.ChangeHealth(-damage);
                Destroy(gameObject);
            }
        }
    }
}
