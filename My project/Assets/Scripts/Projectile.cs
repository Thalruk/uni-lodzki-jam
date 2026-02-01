using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.ChangeHealth(-damage);
                Deactivate();
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Deactivate();
    }

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), 5f);
    }

    void Deactivate()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}