using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public int maxHealth = 5;
    public float speed = 5.0f;
    public float jumpHeight = 10.0f;
    public float jumpDetectDistance = 1f;
    public float attackTime = .5f;
    public float attackCooldownTime = 1f;

    public bool isAttacking = false;
    public bool canAttack = false;

    Ray2D jumpRay;
    Vector2 moveInput = Vector2.zero;

    public GameObject currentWeaponObj;
    Transform weaponSlot;
    PlayerInput input;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        jumpRay = new Ray2D();
        weaponSlot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        //rb.rotation = Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x)  * Mathf.Rad2Deg;

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

    public void Attack()
    {
        if (currentWeaponObj != null && canAttack)
        {
            isAttacking = true;
            currentWeaponObj.transform.GetChild(0).gameObject.SetActive(true);
            canAttack = false;
            StartCoroutine("attackDuration");
        }
    }

    IEnumerator attackDuration()
    {
        yield return new WaitForSeconds(attackTime);

        isAttacking = false;
        currentWeaponObj.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine("attackCooldown");
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime);

        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            collision.gameObject.transform.SetPositionAndRotation(weaponSlot.position, 
                                                        new Quaternion(0, 0, -90f, 90));

            collision.gameObject.transform.SetParent(weaponSlot);

            collision.rigidbody.bodyType = RigidbodyType2D.Kinematic;
            collision.rigidbody.simulated = false;

            collision.collider.enabled = false;

            currentWeaponObj = collision.gameObject;
            canAttack = true;
        }
    }
}
