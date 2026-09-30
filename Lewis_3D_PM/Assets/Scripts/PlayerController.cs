using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int health = 5;
    public int maxHealth = 5;
    public float speed = 5.0f;
    public float sprintBoost = 2.0f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float sprintCost = 25;
    public float staminaRegen = 20;
    public float jumpHeight = 10.0f;


    [Header("Meta Stats")]
    public float sprintCooldown = 1;
    public float jumpDetectDistance = 1f;
    public float interactDistance = 5f;
    public float fusionDmgInterval = 1;
    public bool onGround = true;
    public bool sprinting = false;
    public bool canSprint = true;
    public bool regenStamina = false;
    public bool toggleSprint = true;
    public bool sprintLock = false;
    public bool attacking = false;
    public bool fusionDmg = false;

    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    Vector2 moveInput = Vector2.zero;

    public Weapon currentWeapon;

    Camera playerCam;
    public Transform weaponSlot;
    PlayerInput input;
    Rigidbody rb;
    public GameObject pickupObj;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        jumpRay = new Ray();
        playerCam = Camera.main;

        interactRay = new Ray();
        weaponSlot = playerCam.transform.GetChild(0);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        { 
            
        }

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        onGround = Physics.Raycast(jumpRay, jumpDetectDistance);

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon" || interactHit.collider.tag == "Ammo")
            {
                pickupObj = interactHit.collider.gameObject;
            }

            else
                pickupObj = null;
        }
        else
            pickupObj = null;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && attacking)
                currentWeapon.fire();

        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = moveInput.x * speed;
        tempMove.z = moveInput.y * speed;

        /*
         * If you have some power up enabled, apply the powerup
         * 
         * if (speedBoost)
         * {
         *      tempMove *= speedIncrease;
         * }
         */

        if (sprinting)
        {
            if (stamina > 0)
            {
                tempMove.z *= sprintBoost;

                stamina -= sprintCost * Time.deltaTime;

                StopCoroutine("sprintReset");
                regenStamina = false;

                if(stamina <= 0)
                {
                    canSprint = false;
                    sprinting = false;
                    stamina = 0;
                }
            }
            if (moveInput.y < .75f)
            {
                canSprint = false;
                sprinting = false;
                regenStamina = false;
            }
        }

        if (!sprinting)
        {
            if (!canSprint && !sprintLock)
                StartCoroutine("sprintReset");

            if (canSprint && !regenStamina)
                regenStamina = true;

            if (regenStamina)
            {
                stamina += staminaRegen * Time.deltaTime;

                if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    regenStamina = false;
                }
            }
        }

        rb.linearVelocity = (tempMove.x * transform.right) +
                            (tempMove.y * transform.up) +
                            (tempMove.z * transform.forward);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if(canSprint && (moveInput.y >= .75f) && onGround)
        {
            if (!toggleSprint)
            {
                if (context.ReadValueAsButton())
                    sprinting = true;
                else
                {
                    sprinting = false;
                    canSprint = false;
                }
            }
            else
                if (context.performed)
                    sprinting = !sprinting;
        }
    }

    public void Jump()
    {
        if (onGround)
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pickupObj)
            {
                if (pickupObj.tag == "Weapon")
                {
                    pickupObj.GetComponent<Weapon>().equip(this);
                }

                // Interact to pickup ammo
                /*
                if (pickupObj.tag == "Ammo" && currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
                {
                    int refillAmt = currentWeapon.ammo + currentWeapon.ammoRefill;

                    if (refillAmt >= currentWeapon.maxAmmo)
                    {
                        currentWeapon.ammo = currentWeapon.maxAmmo;
                    }
                    else
                        currentWeapon.ammo += currentWeapon.ammoRefill;

                    Destroy(pickupObj);
                }
                */

                pickupObj = null;
            }
            else if (currentWeapon)
                Reload();
        }
    }

    public void SwitchFireMode()
    {
        if(currentWeapon)
        {
            if(currentWeapon.weaponID == 1)
            {
                currentWeapon.GetComponent<Rifle>().changeFireMode();
            }
        }
    }

    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(currentWeapon)
        {
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    attacking = true;
                else
                    attacking = false;
            }

            else if (context.ReadValueAsButton())
                currentWeapon.fire();
        }
    }

    public void DropWeapon()
    {
        if(currentWeapon)
        {
            currentWeapon.GetComponent<Weapon>().unequip();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ammo" && currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
        {
            int refillAmt = currentWeapon.ammo + currentWeapon.ammoRefill;

            if (refillAmt >= currentWeapon.maxAmmo)
            {
                currentWeapon.ammo = currentWeapon.maxAmmo;
            }
            else
                currentWeapon.ammo += currentWeapon.ammoRefill;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Hazard")
        {
            health--;
        }

        if (collision.gameObject.tag == "FusionHazard")
        {
            health--;
        }

        if (collision.gameObject.tag == "Health" && health < maxHealth)
        {
            health++;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SpeedPower" /* && !speedBoost */)
        {
            // pick up speed boost powerup
            // destroy pickup
            // enable boost
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "FusionHazard")
        { 
            if(!fusionDmg)
            {
                StartCoroutine("fusionDmgCooldown");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LevelEnd")
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "FusionHazard")
        {
            if(fusionDmg)
            {
                StopCoroutine("fusionDmgCooldown");
                fusionDmg = false;
            }
        }
    }

    IEnumerator fusionDmgCooldown()
    {
        fusionDmg = true;

        yield return new WaitForSeconds(fusionDmgInterval);

        health--;
        fusionDmg = false;
    }

    IEnumerator sprintReset()
    {
        sprintLock = true;
        regenStamina = false;

        yield return new WaitForSeconds(sprintCooldown);

        canSprint = true;
        regenStamina = true;
        sprintLock = false;
    }
}
