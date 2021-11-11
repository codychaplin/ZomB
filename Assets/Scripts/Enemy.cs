using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float attackSpeed = 1.5f;
    public int attackDamage = 10;
    float attackDelay = 0;
    Player player;
    Health playerHealth;
    NavMeshAgent agent;
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        // references
        player = Player.instance;
        playerHealth = player.GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        health.InitializeHealth();
        health.OnHit.AddListener(Knockback); // add event listener
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position); // gets distance to target from this
        agent.SetDestination(player.transform.position); // starts moving agent

        if (distance <= agent.stoppingDistance) // if within distance
        {
            FaceTarget(); // update rotation
            AttackTarget(); // attack player
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized; // gets normalized direction
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0001f, direction.z)); // gets rotation to target
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // sets rotation
    }

    void AttackTarget()
    {
        if (Time.time >= attackDelay) // if can attack
        {
            Debug.Log("attacking");
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage); // deal damage

            attackDelay = Time.time + attackSpeed; // set next attack time
        }
    }

    public void Knockback(Vector3 direction, float force)
    {
        agent.velocity = direction * force;
    }
}
