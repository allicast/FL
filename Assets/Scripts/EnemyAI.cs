using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public NavMeshAgent agent;
    public float detectionRange = 10f;
    public float chaseRange = 15f;
    public AudioSource tensionAudio; 
    private int currentPoint = 0;
    private bool chasing = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

       
        float volume = Mathf.Clamp01(1 - (distanceToPlayer / chaseRange));
        tensionAudio.volume = volume;

        if (chasing)
        {
            agent.SetDestination(player.position);

            
            if (distanceToPlayer > chaseRange)
            {
                chasing = false;
                GoToNextPatrolPoint();
            }
        }
        else
        {
            
            if (distanceToPlayer < detectionRange)
            {
                chasing = true;
            }
            else
            {
                
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GoToNextPatrolPoint();
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}