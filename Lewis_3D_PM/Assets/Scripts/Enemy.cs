using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isFollowing = false;

    public int health = 3;
    public int maxHealth = 3;

    public float detectionRange = 5;

    public NavMeshAgent agent;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Vector3.Distance(player.transform.position, transform.position);

        isFollowing = targetDistance <= detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Stop the agent 
            // Do damage to the player
            // Run a coroutine for a cooldown so the enemy doesn't keep trying to attack the player every instance
            // Move if they have to get back to player

            // Set attacking boolean to true
            // In update, make enemy attack and do damage to player while attacking
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Stop attacking
        }
    }
}
