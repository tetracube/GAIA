using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GAIA;

public class TankFSMMuted : MonoBehaviour
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void addNoEvent() { FSMevents.Add((int)Utils.EventTags.NULL); }

    void Start()
    {
        FSM = GAIA_Controller.INSTANCE.m_manager.createMachine(this, (int)FA_Classic.FAType.CLASSIC, "TankAIDeterministic");
        addNoEvent();
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
        }
    }

    private void MoveBack()
    {
        Vector3 movement = m_Speed * Time.deltaTime * transform.forward;
        m_Rigidbody.MovePosition(m_Rigidbody.position - movement);
    }

    public void ExecuteAction(int actionTag)
    {
        m_NavMeshAgent.SetDestination(transform.position);

        switch (actionTag)
        {
            case (int)Utils.ActionTags.APARTAR:
                Aim();
                MoveBack();
                Shoot();
                break;

            case (int)Utils.ActionTags.DISPARAR:
                Aim();
                Shoot();
                break;

            case (int)Utils.ActionTags.MOVERSE:
                FollowEnemy();
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
        Vector3 EnemyPosition = m_Enemy.transform.position;
        m_distance = Vector3.Distance(m_Rigidbody.transform.position, EnemyPosition);

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