using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sr;
    Animator myAnim;

    public bool isFollowing = false;

    public int health = 5;
    public int maxHealth = 5;

    public float speed = 5;
    public float detectionDistance = 5;
    public float stoppingDistance = 1;

    public PlayerController player;
    public Rigidbody2D rb;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Vector2.Distance(player.transform.position, transform.position);

        isFollowing = targetDistance <= detectionDistance;
        myAnim.SetBool("isWalking", isFollowing);

        if (isFollowing)
        {
            if (player.transform.position.x > transform.position.x)
            {
                sr.flipX = false;
                rb.linearVelocityX = speed;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                sr.flipX = true;
                rb.linearVelocityX = -speed;
            }

            if (targetDistance <= stoppingDistance)
            {
                myAnim.SetBool("isWalking", false);
                rb.linearVelocityX = 0;
            }
        }
        else
            rb.linearVelocityX = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MeleeZone")
        {
            health--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Stop the character
            // Attack (do damage)
            // Apply damage cooldown (probably with a coroutine)
            // Resume movement
        }
    }
}
