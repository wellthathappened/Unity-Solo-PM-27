using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Vector2.Distance(player.transform.position, transform.position);

        isFollowing = targetDistance <= detectionDistance;

        if (isFollowing)
        {
            if (player.transform.position.x > transform.position.x)
            {
                rb.linearVelocityX = speed;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                rb.linearVelocityX = -speed;
            }
            
            if (targetDistance <= stoppingDistance)
                rb.linearVelocityX = 0;
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
