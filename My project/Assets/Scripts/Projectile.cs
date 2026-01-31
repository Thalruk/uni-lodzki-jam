using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int speed;

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2);
    }
}
