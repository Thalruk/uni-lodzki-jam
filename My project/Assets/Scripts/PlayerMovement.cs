using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    float horizontal;
    float vertical;
    Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HandleInput();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void HandleMovement()
    {
        Vector2 movementVector = new Vector2(horizontal, vertical);
        rb2D.velocity = movementVector * speed;
    }
}
