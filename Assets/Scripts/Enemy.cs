using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.instance.transform; // sets target to player
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position); // gets distance to target from this
        agent.SetDestination(target.position); // starts moving agent

        if (distance <= agent.stoppingDistance) // if within distance
        {
            FaceTarget(); // update rotation
            AttackTarget(); // attack player
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized; // gets normalized direction
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0001f, direction.z)); // gets rotation to target
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // sets rotation
    }

    void AttackTarget()
    {

    }
}
