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
    public float attackRange = 2f;

    [Header("Velocidades")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3.5f;

    [Header("Audio Enemigo")]
    public AudioSource walkAudio;
    public AudioSource runAudio;
    public AudioSource screamAudio;
    public AudioSource tensionAudio;

    [Header("Audio Player (Ruido)")]
    public float playerNoiseRadius = 8f;   // radio del ruido
    public bool playerIsMakingNoise = false;

    [Header("Animaciones")]
    public Animator animator;

    private int currentPoint = 0;
    private bool chasing = false;
    private bool attacking = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        agent.speed = patrolSpeed;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);

        tensionAudio.volume = 0;
        tensionAudio.Stop();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        HandlePlayerNoiseDetection();
        HandleAudio(distanceToPlayer);

        if (!attacking)
            HandleAI(distanceToPlayer);
    }

    // -----------------------------------------------------------
    // DETECCIÓN DE RUIDO DEL JUGADOR
    // -----------------------------------------------------------
    void HandlePlayerNoiseDetection()
    {
        if (!playerIsMakingNoise) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // si el ruido entra en el radio, activar persecución
        if (distance <= playerNoiseRadius)
        {
            chasing = true;
        }
    }

    // -----------------------------------------------------------
    // AUDIO DEL ENEMIGO (Corre, Camina, Grita y Música Tensa)
    // -----------------------------------------------------------
    void HandleAudio(float distance)
    {
        // Audio de caminar
        if (!chasing && !attacking)
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                if (!walkAudio.isPlaying) walkAudio.Play();
                runAudio.Stop();
            }
            else
            {
                walkAudio.Stop();
            }
        }

        // Audio de correr
        if (chasing && !attacking)
        {
            if (!runAudio.isPlaying) runAudio.Play();
            walkAudio.Stop();
        }

        // Audio de tensión
        if (!chasing)
        {
            if (tensionAudio.isPlaying)
                tensionAudio.Stop();
            return;
        }

        if (distance > chaseRange)
        {
            tensionAudio.volume = 0;
            return;
        }

        if (!tensionAudio.isPlaying)
            tensionAudio.Play();

        float volume = Mathf.Clamp01(1 - (distance / chaseRange));
        tensionAudio.volume = volume;
    }

    // -----------------------------------------------------------
    // LÓGICA DE IA
    // -----------------------------------------------------------
    void HandleAI(float distance)
    {
        if (chasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            SetAnimation("run");

            if (distance > chaseRange)
            {
                chasing = false;
                GoToNextPatrolPoint();
            }
            else if (distance <= attackRange)
            {
                StartCoroutine(AttackSequence());
            }
        }
        else
        {
            if (distance < detectionRange)
            {
                chasing = true;
                return;
            }

            PatrolBehaviour();
        }
    }

    // -----------------------------------------------------------
    // ATAQUE (Grito)
    // -----------------------------------------------------------
    System.Collections.IEnumerator AttackSequence()
    {
        attacking = true;
        agent.isStopped = true;

        animator.SetTrigger("Scream");
        screamAudio.Play();   // ?? Sonido de grito

        yield return new WaitForSeconds(2f);

        agent.isStopped = false;
        attacking = false;
    }

    // -----------------------------------------------------------
    // PATRULLA
    // -----------------------------------------------------------
    void PatrolBehaviour()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPatrolPoint();

        if (agent.velocity.magnitude > 0.1f)
            SetAnimation("walk");
        else
            SetAnimation("idle");
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    // -----------------------------------------------------------
    // ANIMACIONES
    // -----------------------------------------------------------
    void SetAnimation(string state)
    {
        animator.SetBool("isWalking", state == "walk");
        animator.SetBool("isRunning", state == "run");
        animator.SetBool("isIdle", state == "idle");
    }
}