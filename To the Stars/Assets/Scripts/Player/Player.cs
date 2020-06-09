using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
    #region ATTRIBUTES
    private PlayerState m_playerState     = PlayerState.Idle;   // Current state
    public  Lane        m_currentLane;                          // The Lane the character is at. Set on the inspector the init Lane
    public  Animator    m_playerAnimator;
    
    // To get movement direction
    private Vector3     m_prevPosition;
    private Vector3     m_movementDirection;

    private PlayerState m_attemptingState;                      // Holds a state that the player failed to change the last attempt
    private bool        m_isAttemptingStateAlive = false;       // If that state could still be considered to enter

    // State components
    private JumpState  m_jumpState;
    private IdleState  m_idleState;
    private DashState  m_dashState;

    // Actions for the current state
    Action m_enterStateAction;
    Action m_updateStateAction;
    Action m_exitStateAction;

    // Dictionaries gathering all the actions for each state
    Dictionary<PlayerState, Action> m_enterStateActions;
    Dictionary<PlayerState, Action> m_updateStateActions;
    Dictionary<PlayerState, Action> m_exitStateActions;
    #endregion

    #region PROPERTIES
    public PlayerState PLAYER_STATE
    {
        get { return m_playerState;  }
    }

    public Lane CURRENT_LANE
    {
        get { return m_currentLane; }
    }

    public Vector3 MOVEMENT_DIR
    {
        get { return m_movementDirection; }
    }
    #endregion

    void Start ()
    {
        m_prevPosition = transform.position;

        // Place character at init Lane
        transform.position = new Vector3(transform.position.x, m_currentLane.transform.position.y, transform.position.z);

        // Subscribe to input manager for handling the input
        InputManager.OnInputEvent += HandleInput;

        // Subscribe to GameOver event
        GameManager.OnGameOver += OnGameOver;

        // Get state components, initialize dictionaries with the functions from the different states
        // and set the current actions to perform with the ones corresponding to the current state
        GetStates();
        InitializeDictionaries();
        UpdateActions();

        // Perform the enter function of init state
        m_enterStateAction();
	}

    void GetStates()
    {
        // Get state components
        m_jumpState = GetComponent<JumpState>();
        m_idleState = GetComponent<IdleState>();
        m_dashState = GetComponent<DashState>();
    }

    void InitializeDictionaries()
    {
        m_enterStateActions  = new Dictionary<PlayerState, Action>();
        m_updateStateActions = new Dictionary<PlayerState, Action>();
        m_exitStateActions   = new Dictionary<PlayerState, Action>();

        // Initialize dictionaries with functions for each player's state
        #region JUMP_FUNCTIONS
        m_enterStateActions.Add(PlayerState.JumpingUp, m_jumpState.Enter);
        m_updateStateActions.Add(PlayerState.JumpingUp, m_jumpState.UpdateState);
        m_exitStateActions.Add(PlayerState.JumpingUp, m_jumpState.Exit);

        m_enterStateActions.Add(PlayerState.JumpingDown, m_jumpState.Enter);
        m_updateStateActions.Add(PlayerState.JumpingDown, m_jumpState.UpdateState);
        m_exitStateActions.Add(PlayerState.JumpingDown, m_jumpState.Exit);
        #endregion

        #region IDLE_FUNCTIONS
        m_enterStateActions.Add(PlayerState.Idle, m_idleState.Enter);
        m_updateStateActions.Add(PlayerState.Idle, m_idleState.UpdateState);
        m_exitStateActions.Add(PlayerState.Idle, m_idleState.Exit);
        #endregion

        #region DASH_FUNCTIONS
        m_enterStateActions.Add(PlayerState.DashingUp, m_dashState.Enter);
        m_updateStateActions.Add(PlayerState.DashingUp, m_dashState.UpdateState);
        m_exitStateActions.Add(PlayerState.DashingUp, m_dashState.Exit);

        m_enterStateActions.Add(PlayerState.DashingDown, m_dashState.Enter);
        m_updateStateActions.Add(PlayerState.DashingDown, m_dashState.UpdateState);
        m_exitStateActions.Add(PlayerState.DashingDown, m_dashState.Exit);
        #endregion


    }

    void UpdateActions()
    {
        // Set actions to perform according to player's state
        m_enterStateAction  = m_enterStateActions[m_playerState];
        m_updateStateAction = m_updateStateActions[m_playerState];
        m_exitStateAction   = m_exitStateActions[m_playerState];
    }
	
	void Update ()
    {
        if (m_isAttemptingStateAlive)
        {
            m_isAttemptingStateAlive = false;
            ChangePlayerState(m_attemptingState);
        }

        // Get movement direction
        m_movementDirection = (transform.position - m_prevPosition);
        m_prevPosition = transform.position;

        m_updateStateAction();
	}

    /// <summary>
    /// It handles the player's input
    /// </summary>
    void HandleInput(InputType input)
    {
        // Previous attemp to change state due to an input is not loger valid. It would be replaced by this new input
        m_isAttemptingStateAlive = false;

        switch (input)
        {
            case InputType.JumpUp:
                ChangePlayerState(PlayerState.JumpingUp);
                break;

            case InputType.JumpDown:
                ChangePlayerState(PlayerState.JumpingDown);
                break;

            case InputType.DashUp:
                ChangePlayerState(PlayerState.DashingUp);
                break;

            case InputType.DashDown:
                ChangePlayerState(PlayerState.DashingDown);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// It changes the player's state
    /// It acts as a FSM that will take charge of changing state if possible
    /// </summary>
    public void ChangePlayerState(PlayerState playerState)
    {
        bool stateChangeAllowed = false;

        #region CHECK_STATE_CHANGE_ALLOWED
        switch (playerState)
        {
            case PlayerState.Idle:
                stateChangeAllowed = true;
                m_playerAnimator.SetTrigger("Idle");
                break;

            case PlayerState.JumpingDown:
                // JumpingDown is only allowed from idle state and not being on bottom lane
                if (m_playerState == PlayerState.Idle && m_currentLane.m_lanePos != LanePos.Bottom)
                {
                    stateChangeAllowed = true;
                    m_playerAnimator.SetTrigger("JumpDown");
                }
                break;

            case PlayerState.JumpingUp:
                // JumpingUp is only allowed from idle state and not being on top lane
                if (m_playerState == PlayerState.Idle && m_currentLane.m_lanePos != LanePos.Top)
                {
                    stateChangeAllowed = true;
                    m_playerAnimator.SetTrigger("JumpUp");
                }
                break;

            case PlayerState.DashingDown:
                // Dashing Down is always allowed if you are not already dashing
                if (m_playerState != PlayerState.DashingDown && m_playerState != PlayerState.DashingUp)
                {
                    stateChangeAllowed = true;
                    m_playerAnimator.SetTrigger("DashDown");
                }
                break;

            case PlayerState.DashingUp:
                // Dashing Up is always allowed if you are not already dashing
                if (m_playerState != PlayerState.DashingDown && m_playerState != PlayerState.DashingUp)
                {
                    stateChangeAllowed = true;
                    m_playerAnimator.SetTrigger("DashUp");
                }
                break;

            default:
                break;
        }
        #endregion

        #region CHANGE_STATE
        if (stateChangeAllowed)
        {
            // Change state
            m_playerState = playerState;

            // Execute exit function before leaving the current state
            m_exitStateAction();

            // Update actions to point to the new ones from the new state
            UpdateActions();

            // Execute enter function of this new state
            m_enterStateAction();
        }
        else
        {
            // Keep track of last attemp of state change
            m_attemptingState = playerState;
            m_isAttemptingStateAlive = true;
        }
        #endregion
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If it collided with a lane, update its current lane
        if (other.CompareTag("Lane"))
        {
            m_currentLane = other.GetComponent<Lane>();
        }
    }

    void OnGameOver()
    {
        // Disable player's movement
        GetComponent<ForwardMovement>().enabled = false;
        enabled = false;
    }

    void OnDisable()
    {
        InputManager.OnInputEvent -= HandleInput;
        GameManager.OnGameOver -= OnGameOver;
    }
}