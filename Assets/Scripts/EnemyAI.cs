using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("IA")]
    public Transform[] patrolPoints;
    public Transform player;
    public NavMeshAgent agent;
    public float detectionRange = 10f;
    public float chaseRange = 15f;
    public float attackRange = 2f; // rango para gritar

    [Header("Audio")]
    public AudioSource tensionAudio;

    [Header("Animaciones")]
    public Animator animator;

    private int currentPoint = 0;
    private bool chasing =false;
    private bool attacking = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Ajustar volumen del audio según la distancia
        float volume = Mathf.Clamp01(1 - (distanceToPlayer / chaseRange));
        tensionAudio.volume = volume;

        if (attacking)
            return; // Si está gritando, no hacer nada más

        if (chasing)
        {
            agent.SetDestination(player.position);

            // Animación de correr
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);

            if (distanceToPlayer > chaseRange)
            {
                chasing = false;
                GoToNextPatrolPoint();
            }
            else if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(AttackSequence());
            }
        }
        else
        {
            // Detecta al jugador
            if (distanceToPlayer < detectionRange)
            {
                chasing = true;
            }
            else
            {
                // Patrulla normal
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GoToNextPatrolPoint();

                // Animación de caminar o idle
                if (agent.velocity.magnitude > 0.1f)
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isIdle", false);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isIdle", true);
                }
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    System.Collections.IEnumerator AttackSequence()
    {
        attacking = true;
        agent.isStopped = true;

        // Animación de grito
        animator.SetTrigger("Scream");

        // Espera la duración del grito antes de volver a moverse
        yield return new WaitForSeconds(2f);

        agent.isStopped = false;
        attacking = false;
    }
}