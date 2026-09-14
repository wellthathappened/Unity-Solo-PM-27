using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 10.0f;
    public float jumpDetectDistance = 1f;

    Ray2D jumpRay;
    Vector2 moveInput = Vector2.zero;

    PlayerInput input;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        jumpRay = new Ray2D();
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        rb.linearVelocityX = moveInput.x * speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput.x = context.ReadValue<Vector2>().x;
    }

    public void Jump()
    {
        if (Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpDetectDistance))
            rb.AddForceY(jumpHeight, ForceMode2D.Impulse);
    }
}
