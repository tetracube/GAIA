using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GAIA;

public class CharacterAIFSM : MonoBehaviour
{
    public int m_PlayerNumber = 1;                                // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                                   // How fast the tank moves forward and back.
   
    private int noShooting = 0;                                   // Counter to avoid shooting at spawn.
    private int noShootingAgain = 0;                              // Counter to avoid shooting too fast.
    private Rigidbody m_Rigidbody;                                // Reference used to move the tank.
    private float distance;                                       // Store the distance between the player and the tank.

    //IA attributes
    private FSM_Machine FSM;                                      // Variable que contiene la FSM.
    private GAIA_Manager manager;                                 // Variable que contiene la referencia al manager.
    private List<int> FSMactions;                                 // Variable que contiene las acciones a realizar en cada update.
    private List<int> FSMevents = new List<int>();                // Variable que contiene los eventos.

    private void Awake()
    {
        m_Rigidbody     = GetComponent<Rigidbody>();
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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void addNoEvent() { FSMevents.Add((int)Utils.EventTags.NULL); }

    // Start is called before the first frame update
    void Start()
    {
        manager = GAIA_Controller.INSTANCE.m_manager;
        FSM = manager.createMachine(this, (int)FA_Classic.FAType.CLASSIC, "AIDeterministic");
        addNoEvent();
    }

    private void Move()
    {
        Vector3 PlayerPosition = GameObject.Find("EnemyCharacter").transform.position;
    }

    private void Aim()
    {
        var damping = 2;
        Vector3 PlayerPosition = GameObject.Find("EnemyCharacter").transform.position;
        var lookPos = PlayerPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void Shoot()
    {
        if (noShooting > 150 && noShootingAgain > 60)
            noShootingAgain = 0;
    }

    private void moveBack()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
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

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPosition = GameObject.Find("EnemyCharacter").transform.position;
        distance = Vector3.Distance(m_Rigidbody.transform.position, PlayerPosition);

        noShooting++;
        noShootingAgain++;

        m_Rigidbody.isKinematic = false;

        FSMactions = FSM.Update();
        for (int i = 0; i < FSMactions.Count; i++)
            if (FSMactions[i] != (int)Utils.ActionTags.NULL)
                ExecuteAction(FSMactions[i]);
    }
}