using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isFollowing = false;

    public int health = 5;
    public int maxHealth = 5;

    public float speed = 5;
    public float range = 5;

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

        if (Mathf.Abs(player.transform.position.x - transform.position.x) <= range)
            isFollowing = true;
        else
            isFollowing = false;



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
