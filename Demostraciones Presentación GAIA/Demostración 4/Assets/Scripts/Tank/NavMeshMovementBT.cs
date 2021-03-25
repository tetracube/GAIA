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
 //   private GameManager gameManager;
    private string target = "Tank(Clone)";
//    private TankManager targetNext;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TankShooting = GetComponent<TankShooting>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
        manager.changeBT(this.gameObject, BTFileName);
    }

    // Start is called before the first frame update
    void Start()
    {
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
            Vector3 PlayerPosition = GameObject.Find(target).transform.position;
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
        Vector3 PlayerPosition = GameObject.Find(target).transform.position;
        var lookPos = PlayerPosition - transform.position;
        lookPos.y = 0;
        if (lookPos.x != 0 && lookPos.z != 0)
        {
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
        return true;
    }

    [Task]
    bool Shoot()
    {
        if (noShooting > 150 && noShootingAgain > 60)
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
    bool ChangeColor()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        Color random = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = random;
        }

        return true;
    }

    [Task]
    bool GrowRandom()
    {
        Transform scale = GetComponentInParent<Transform>();

        float scaleFactor = Random.Range(-0.6f, 0.6f);

        Vector3 newScale = scale.localScale;

        newScale.x = newScale.x + scaleFactor;
        newScale.y = newScale.y + scaleFactor;
        newScale.z = newScale.z + scaleFactor;

        scale.localScale = newScale;

        return true;
    }

    [Task]
    bool Teleport()
    {
        Transform position = GetComponentInParent<Transform>();

        float positionX = Random.Range(-25f, 25f);
        float positionZ = Random.Range(-25f, 25f);

        Vector3 newPosition = position.localPosition;

        newPosition.x = positionX;
        newPosition.z = positionZ;

        position.position = newPosition;
        

        return true;
    }

    //[Task]
    //bool ChangeTarget()
    //{
    //    gameManager = GameObject.FindObjectOfType<GameManager>();

    //    targetNext = gameManager.getActiveTank();

    //    target = targetNext.m_Instance.name;

    //    return true;
    //}

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
    void LateUpdate()
    {
        PlayerPosition = GameObject.Find(target).transform.position;
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
    }
}
