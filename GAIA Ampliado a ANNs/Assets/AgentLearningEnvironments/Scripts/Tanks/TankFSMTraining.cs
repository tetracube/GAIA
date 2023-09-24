using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GAIA;

public class TankFSMTraining : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public NavMeshAgent m_NavMeshAgent;
    public GameObject m_Enemy;

    private float spawnTime = 0f;
    private float shootTime = 0f;
    private readonly float spawnShootCooldown = 1f;
    private readonly float shootCooldown = 0.4f;
    private Rigidbody m_Rigidbody;
    private float m_distance;
    private TankShooting m_TankShooting;

    private FSM_Machine FSM;
    private List<int> FSMactions;
    private readonly List<int> FSMevents = new();

    private IAgent m_Agent;
    private ImitationManager m_ImitationManager;

    private void Awake()
    {
        m_Rigidbody     = GetComponent<Rigidbody>();
        m_TankShooting  = GetComponent<TankShooting>();
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void addNoEvent() { FSMevents.Add((int)Utils.EventTags.NULL); }

    void Start()
    {
        GAIA_Manager manager = GAIA_Controller.INSTANCE.m_manager;
        m_ImitationManager = FindObjectOfType<ImitationManager>();

        FSM = manager.createMachine(this, (int)FA_Classic.FAType.CLASSIC, "TankAIDeterministic");
        addNoEvent();

        m_Agent = manager.createAgent(gameObject, "Tank");
        m_Agent.SetOnActionsReceived(RewardFromFSMActions);
        m_Agent.SetOnAddObservations(AddObservations);

        void AddObservations()
        {
            m_Agent.AddObservation(m_distance);
        }

        void RewardFromFSMActions(float[] ContinuousActions, int[] DiscreteActions)
        {
            int CorrectImitations = 0;
            int TotalImitations = 0;


            if (IsCorrectAction(DiscreteActions[0], (int)Utils.ActionTags.APARTAR))
            {
                m_Agent.AddReward(1f);
                CorrectImitations++;
            }
            else
            {
                m_Agent.AddReward(-1f);
            }
            TotalImitations++;

            if (IsCorrectAction(DiscreteActions[1], (int)Utils.ActionTags.DISPARAR))
            {
                m_Agent.AddReward(1f);
                CorrectImitations++;
            }
            else
            {
                m_Agent.AddReward(-1f);
            }
            TotalImitations++;

            if (IsCorrectAction(DiscreteActions[2], (int)Utils.ActionTags.MOVERSE))
            {
                m_Agent.AddReward(1f);
                CorrectImitations++;
            }
            else
            {
                m_Agent.AddReward(-1f);
            }
            TotalImitations++;

            if (m_ImitationManager)
            {
                m_ImitationManager.AddImitationSample(CorrectImitations, TotalImitations);
            }
        }
    }

    private bool IsCorrectAction(int RNNAction, int FSMActionTag)
    {
        return (RNNAction == 1 && FSMactions.Contains(FSMActionTag)) ||
            (RNNAction == 0 && !FSMactions.Contains(FSMActionTag));
    }

    public void EndEpisode()
    {
        m_Agent.EndEpisode();
    }

    private void Move()
    {
        Vector3 PlayerPosition = m_Enemy.transform.position;
        m_NavMeshAgent.SetDestination(PlayerPosition);
    }

    private void Aim()
    {
        var damping = 2;
        Vector3 PlayerPosition = m_Enemy.transform.position;
        var lookPos = PlayerPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void Shoot()
    {
        float CurrentTime = Time.time;
        if ((CurrentTime - spawnTime) > spawnShootCooldown && (CurrentTime - shootTime) > shootCooldown)
        {
            shootTime = CurrentTime;
            m_TankShooting.FireAI();
        }
    }

    private void moveBack()
    {
        Vector3 movement = transform.forward * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position - movement);
    }

    public void ExecuteAction(int actionTag)
    {
        switch (actionTag)
        {
            case (int)Utils.ActionTags.APARTAR:
                Aim();
                moveBack();
                Shoot();
                break;

            case (int)Utils.ActionTags.DISPARAR:
                Aim();
                Shoot();
                break;

            case (int)Utils.ActionTags.MOVERSE:
                Move();
                break;
        }
    }

    public List<int> BuscarEventos()
    {
        FSMevents.Clear();

        if (m_distance < 5)
        {
            FSMevents.Add((int)Utils.EventTags.DEMASIADO_CERCA);
        }

        if (m_distance > 17)
        {
            FSMevents.Add((int)Utils.EventTags.DISTANCIA_NO_OK);
        }

        if (m_distance <= 17 && m_distance >= 5)
        {
            FSMevents.Add((int)Utils.EventTags.DISTANCIA_OK);
        }

        if (FSMevents.Count == 0)
        {
            FSMevents.Add((int)Utils.EventTags.NULL);
        }

        return FSMevents;
    }

    void FixedUpdate()
    {
        if (m_Enemy != null)
        {
            m_distance = Vector3.Distance(m_Rigidbody.transform.position, m_Enemy.transform.position);
        }

        m_Rigidbody.isKinematic = false;
        FSMactions = FSM.Update();
        for (int i = 0; i < FSMactions.Count; i++)
        {
            if (FSMactions[i] != (int)Utils.ActionTags.NULL)
            {
                ExecuteAction(FSMactions[i]);
            }
        }
    }
}