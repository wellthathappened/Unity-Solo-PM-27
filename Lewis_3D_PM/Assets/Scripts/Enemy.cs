using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isFollowing = false;
    public bool isAttacking = false;
    public bool canAttack = true;
    public bool waitToAttack = false;

    public int health = 3;
    public int maxHealth = 3;

    public int damage = 1;

    public float attackCoolTime = 1;
    public float attackKnockback = 10;

    public float detectionRange = 5;

    public NavMeshAgent agent;
    public PlayerController player;
    public GameObject bloodParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().enemyCount--;

            Destroy(gameObject);
        }

        float targetDistance = Vector3.Distance(player.transform.position, transform.position);

        isFollowing = targetDistance <= detectionRange;

        if (isFollowing && health > 0)
        {
            agent.destination = player.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && canAttack)
        {
            agent.isStopped = true;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(attackKnockback, transform.forward, 1);
            player.health -= damage;
            canAttack = false;
            StartCoroutine("AttackCooldown");

            // Stop the agent 
            // Do damage to the player
            // Run a coroutine for a cooldown so the enemy doesn't keep trying to attack the player every instance
            // Move if they have to get back to player

            // Set attacking boolean to true
            // In update, make enemy attack and do damage to player while attacking
        }

        if(collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            GameObject p = Instantiate(bloodParticle, collision.collider.ClosestPoint(collision.gameObject.transform.position), Quaternion.Inverse(collision.gameObject.transform.rotation), transform);
            Destroy(p, .33f);
            health--;
        }
    }

    IEnumerator AttackCooldown()
    {
        waitToAttack = true;

        yield return new WaitForSeconds(attackCoolTime);

        canAttack = true;
        waitToAttack = false;
        agent.isStopped = false;
    }
}
