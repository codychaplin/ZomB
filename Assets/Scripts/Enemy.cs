using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public Transform rayStart;
    public float attackSpeed = 1.5f;
    public int attackDamage = 10;
    public float baseStoppingDistance = 1.8f;
    float attackDelay = 0;
    bool isTargetPlayer = true;
    Player player;
    NavMeshPath path;
    NavMeshAgent agent;
    Health health;
    Health playerHealth;
    Health targetHealth;
    Vector3 target;
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        // references
        player = Player.instance;
        playerHealth = player.GetComponent<Health>();
        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        
        // debug purposes
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = 0;

        health.OnHit.AddListener(Knockback); // add event listener

        agent.stoppingDistance = baseStoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath)
            DrawPath();
        
        // if can't find path to player, set target to obstacle in the way
        if (isTargetPlayer && agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Vector3 direction = player.transform.position - rayStart.position;
            int mask = 1 << LayerMask.NameToLayer("Obstacle");
            if (Physics.Raycast(rayStart.position, direction, out RaycastHit hit, 1000f, mask))
            {
                if (hit.collider.TryGetComponent(out Health health))
                {
                    target = hit.collider.transform.position; // sets target
                    targetHealth = health; // sets target's health
                    agent.stoppingDistance = 1f;
                    agent.SetDestination(target);
                    isTargetPlayer = false;
                }
            }
        }

        if (isTargetPlayer) // if player is current target
        {
            target = player.transform.position; // sets target
            agent.SetDestination(target); // starts moving agent
            float distance = Vector3.Distance(target, transform.position); // gets distance to target from this

            if (distance <= agent.stoppingDistance) // if within distance
            {
                FaceTarget(); // update rotation
                AttackTarget(playerHealth); // attack player
            }
        }
        else // if target is obstacle
        {
            agent.CalculatePath(player.transform.position, path); // try to calculate path to player
            if (path.status == NavMeshPathStatus.PathComplete) // if successful, set target to player
            {
                agent.stoppingDistance = baseStoppingDistance;
                isTargetPlayer = true;
            }
            else // if still no path, keep obstacle as target
            {
                float distance = Vector3.Distance(target, transform.position); // gets distance to target from this
                if (distance <= agent.stoppingDistance) // if within distance
                {
                    if (targetHealth != null) // if target has health, attack
                        AttackTarget(targetHealth);
                    else // otherwise, set target to player
                    {
                        agent.stoppingDistance = baseStoppingDistance;
                        isTargetPlayer = true;
                    }
                }
            }
        }
    }

    void DrawPath()
    {
        line.positionCount = agent.path.corners.Length;
        if (line.positionCount < 1) line.positionCount = 1;
        line.SetPosition(0, transform.position);

        if (agent.path.corners.Length < 2)
            return;

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            Vector3 pos = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
            line.SetPosition(i, pos);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized; // gets normalized direction
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0001f, direction.z)); // gets rotation to target
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // sets rotation
    }

    void AttackTarget(Health target)
    {
        if (Time.time >= attackDelay) // if can attack
        {
            if (target != null)
                target.TakeDamage(attackDamage); // deal damage

            attackDelay = Time.time + attackSpeed; // set next attack time
        }
    }

    public void Knockback(Vector3 direction, float force)
    {
        agent.velocity = direction * force;
    }
}
