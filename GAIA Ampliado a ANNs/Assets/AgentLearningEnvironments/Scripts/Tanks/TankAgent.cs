using UnityEngine;
using UnityEngine.AI;
using GAIA;

public class TankAgent : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public NavMeshAgent m_NavMeshAgent;
    public GameObject m_Enemy;

    private Rigidbody m_Rigidbody;
    private TankShooting m_TankShooting;
    private IAgent m_Agent;
    private float m_DistanceToPlayer;
    private string m_Behavior = "Tank";

    private float spawnTime = 0f;
    private float shootTime = 0f;
    private readonly float spawnShootCooldown = 1f;
    private readonly float shootCooldown = 0.4f;

    private float m_PreviousEnemyHealth = 0f;
    private float m_PreviousHealth = 0f;

    public void SetEnemy(GameObject Enemy)
    {
        m_Enemy = Enemy;
        m_PreviousEnemyHealth = m_Enemy.GetComponent<TankHealth>().GetCurrentHealth();
    }

    public void SetBehavior(string Behavior)
    {
        m_Behavior = Behavior;
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TankShooting = GetComponent<TankShooting>();
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        spawnTime = Time.time;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    void Start()
    {
        m_Agent = GAIA_Controller.INSTANCE.m_manager.createAgent(gameObject, m_Behavior);
        m_Agent.SetOnActionsReceived(ExecuteActions);
        m_Agent.SetOnAddObservations(AddObservations);

        void AddObservations()
        {
            m_Agent.AddObservation(m_DistanceToPlayer);
        }

        void ExecuteActions(float[] ContinuousActions, int[] DiscreteActions)
        {
            m_NavMeshAgent.SetDestination(transform.position);

            if (DiscreteActions[0] == 1)
            {
                Aim();
                MoveBack();
            }

            if (DiscreteActions[1] == 1)
            {
                Aim();
                Shoot();
            }

            if (DiscreteActions[2] == 1)
            {
                FollowEnemy();
            }
        }
    }

    private void FollowEnemy()
    {
        Vector3 EnemyPosition = m_Enemy.transform.position;
        m_NavMeshAgent.SetDestination(EnemyPosition);
    }

    private void Aim()
    {
        Vector3 EnemyPosition = m_Enemy.transform.position;
        Vector3 lookPos = EnemyPosition - transform.position;
        lookPos.y = 0;
        if (lookPos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            float damping = 2;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }

    private void Shoot()
    {
        float CurrentTime = Time.time;
        if ((CurrentTime - spawnTime) > spawnShootCooldown && (CurrentTime - shootTime) > shootCooldown)
        {
            shootTime = CurrentTime;
            m_TankShooting.FireAI();
            m_Agent.AddReward(-0.5f);
        }
    }

    private void MoveBack()
    {
        Vector3 movement = m_Speed * Time.deltaTime * transform.forward;
        m_Rigidbody.MovePosition(m_Rigidbody.position - movement);
    }

    public void EndEpisode()
    {
        float CurrentEnemyHealth = m_Enemy.GetComponent<TankHealth>().GetCurrentHealth();
        if (CurrentEnemyHealth <= 0)
        {
            m_Agent.AddReward(500f);
        }
        float CurrentHealth = GetComponent<TankHealth>().GetCurrentHealth();
        if (CurrentHealth <= 0)
        {
            m_Agent.AddReward(-500f);
        }

        m_PreviousEnemyHealth = m_Enemy.GetComponent<TankHealth>().m_StartingHealth;
        m_PreviousHealth = GetComponent<TankHealth>().m_StartingHealth;

        m_Agent.EndEpisode();
    }

    void FixedUpdate()
    {
        Vector3 EnemyPosition = m_Enemy.transform.position;
        m_DistanceToPlayer = Vector3.Distance(m_Rigidbody.transform.position, EnemyPosition);

        m_Agent.AddReward(-0.1f);
        float CurrentEnemyHealth = m_Enemy.GetComponent<TankHealth>().GetCurrentHealth();
        if (CurrentEnemyHealth != m_PreviousEnemyHealth)
        {
            if (CurrentEnemyHealth != 0)
            {
                m_Agent.AddReward(m_PreviousEnemyHealth - CurrentEnemyHealth);
            }
            m_PreviousEnemyHealth = CurrentEnemyHealth;
        }

        float CurrentHealth = GetComponent<TankHealth>().m_StartingHealth;
        if (CurrentHealth != m_PreviousHealth)
        {
            if (CurrentHealth != 0)
            {
                m_Agent.AddReward(m_PreviousHealth - CurrentHealth);
            }
            m_PreviousHealth = CurrentHealth;
        }
    }
}