#define PANDA

using UnityEngine;
using UnityEngine.AI;
using Panda;
using GAIA;

public class NavMeshMovementBT : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public NavMeshAgent agent;                  // The NavMeshAgent attached to the tank.
    public string BTFileName;                   // Choose the BT to load by txt file name.

    private int noShooting = 0;                 // Counter to avoid shooting at spawn.
    private int noShootingAgain = 0;            // Counter to avoid shooting too fast
    private int noMoveAgain = 30;               // Counter to avoid recalculating NavMesh destination too often.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private Vector3 previousPosition;           // Store the transform of the previous position to check is the tank is moving.
    private TankShooting m_TankShooting;        // Reference to the shooting script.
    private float distance;                     // Variable to store the distance between tanks.
    private Vector3 PlayerPosition;             // Variable to store the enemy tanks position.
    private GAIA_Manager manager;               // Instatiates the manager.
    private int frame;                          // Frame counter to determine time past.
    private bool spinToWin;                     // Bool to determine whether the tank spins or not.
    private float degreesPerSecond = 200;        // Magnitude of spin of death rotation.

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TankShooting = GetComponent<TankShooting>();
        frame = 0;
        spinToWin = false;
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;
        #if (PANDA)
            manager.changeBT(this.gameObject, BTFileName);
        #endif
        frame = 0;
        spinToWin = false;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
        #if (PANDA)
            manager.changeBT(this.gameObject, BTFileName);
        #endif
        frame = 0;
        spinToWin = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        spinToWin = false;
        frame = 0;
        previousPosition = m_Rigidbody.transform.position;
        manager = GAIA_Controller.INSTANCE.m_manager;
        #if (PANDA)
        manager.createBT(this.gameObject, BTFileName);
        #endif
    }

    [Task]
    bool Move()
    {
        if (noMoveAgain >= 30)
        {
            Vector3 PlayerPosition = GameObject.Find("Tank(Clone)").transform.position;
            agent.SetDestination(PlayerPosition);
            noMoveAgain = 0;
            return true;
        }
        return false;
    }

    [Task]
    bool Aim()
    {
        var damping = 2;
        Vector3 PlayerPosition = GameObject.Find("Tank(Clone)").transform.position;
        var lookPos = PlayerPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        return true;
    }

    [Task]
    bool ActivateSpin()
    {
        spinToWin = true;
        return true;
    }

    [Task]
    bool StopSpin()
    {
        spinToWin = false;
        return true;
    }

    [Task]
    bool Shoot()
    {
        if (noShooting > 240 && noShootingAgain > 60)
        {
            m_TankShooting.FireAI();
            noShootingAgain = 0;
            return true;
        }
        return false;
        
    }

    [Task]
    bool ShootNonStop()
    {
        if (noShooting > 240 && noShootingAgain > 5)
        {
            m_TankShooting.FireAI();
            noShootingAgain = 0;
            return true;
        }
        return false;

    }

    [Task]
    bool MoveBack()
    {

        {// Use this for initialization 
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position - movement);

            return true;
        }
    }

    [Task]
    bool tooFar()
    {
        if (distance >= 17)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Task]
    bool tooClose()
    {
        if (distance <= 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Task]
    bool isStopped()
    {
        if (previousPosition == m_Rigidbody.transform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Task]
    bool shootingRange()
    {
        if (distance >= 5 && distance <= 17)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frame++;
        PlayerPosition = GameObject.Find("Tank(Clone)").transform.position;
        distance = Vector3.Distance(m_Rigidbody.transform.position, PlayerPosition);
        noShooting++;
        noShootingAgain++;
        noMoveAgain++;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.isKinematic = false;
        if (previousPosition != m_Rigidbody.transform.position)
        {
            previousPosition = m_Rigidbody.transform.position;
        }
        if (spinToWin)
        {
            transform.Rotate(Vector3.up * degreesPerSecond * Time.deltaTime, Space.Self);
            transform.Rotate(Vector3.left * degreesPerSecond * Time.deltaTime, Space.Self);
        }
        if (frame == 600)
        {
            #if (PANDA)
            manager.changeBT(this.gameObject, "BTDoom");
            frame = -100000;
            #endif
        }
    }
}
