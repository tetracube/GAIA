using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GAIA;
using static Utils;

public class NavMeshMovementFSM : MonoBehaviour
{
    public int m_PlayerNumber = 1;                                // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                                   // How fast the tank moves forward and back.
    public AudioSource m_MovementAudio;                           // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;                              // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;                             // Audio to play when the tank is moving.
    public NavMeshAgent m_NavMeshAgent;                           // The NavMeshAgent attached to the tank.
    public GameObject m_Enemy;

    private float spawnTime = 0f;
    private float shootTime = 0f;
    private readonly float spawnShootCooldown = 1f;
    private readonly float shootCooldown = 0.4f;
    private Rigidbody m_Rigidbody;                                // Reference used to move the tank.
    private float distance;                                       // Store the distance between the player and the tank.
    private TankShooting m_TankShooting;                          // Reference to the shooting script.

    //IA attributes
    private FSM_Machine FSM;                                      // Variable que contiene la FSM.
    private GAIA_Manager manager;                                 // Variable que contiene la referencia al manager.
    private List<int> FSMactions;                                 // Variable que contiene las acciones a realizar en cada update.
    private List<int> FSMevents = new List<int>();                // Variable que contiene los eventos.

    private void Awake()
    {
        m_Rigidbody     = GetComponent<Rigidbody>();
        m_TankShooting  = GetComponent<TankShooting>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;
        spawnTime = Time.time;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void addNoEvent() { FSMevents.Add((int)Utils.EventTags.NULL); }

    // Start is called before the first frame update
    void Start()
    {
        manager = GAIA_Controller.INSTANCE.m_manager;
        FSM = manager.createMachine(this, (int)FA_Classic.FAType.CLASSIC, "TankAIDeterministic");
        addNoEvent();
    }

    private void Move()
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
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = m_Speed * Time.deltaTime * transform.forward;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position - movement);
    }

    public void ExecuteAction(int actionTag)
    {
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
                Move();
                break;
        }
    }

    public List<int> BuscarEventos()
    {
        FSMevents.Clear();

        if (distance < 5)
        {
            FSMevents.Add((int)Utils.EventTags.DEMASIADO_CERCA);
        }

        if (distance > 17)
        {
            FSMevents.Add((int)Utils.EventTags.DISTANCIA_NO_OK);
        }

        if (distance <= 17 && distance >= 5)
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
        distance = Vector3.Distance(m_Rigidbody.transform.position, EnemyPosition);

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