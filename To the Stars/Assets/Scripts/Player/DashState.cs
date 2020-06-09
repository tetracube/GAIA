using UnityEngine;
using System.Collections;

public class DashState : MonoBehaviour, IState
{
    private float       m_timeDashStart;
    public float        m_dashDuration;

    private Vector3     m_originalPos;
    private Transform   m_transform;

    private Player      m_playerScript;

    float elapsedTime;
    float nextYPos;
    float finalYPos;

    void Start()
    {
        m_playerScript  = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_transform     = transform;
    }

    public void Enter()
    {
        m_timeDashStart = Time.time;
        m_originalPos   = m_transform.position;

        // Get the position trying to reach according to the state
        finalYPos       = (m_playerScript.PLAYER_STATE == PlayerState.DashingUp) ? Lanes.INSTANCE.m_topLane.transform.position.y : Lanes.INSTANCE.m_bottomLane.transform.position.y;
    }

    public void UpdateState()
    {
        if (m_playerScript.PLAYER_STATE == PlayerState.DashingUp)
        {
            ControlDash(Direction.Up);
        }
        else if (m_playerScript.PLAYER_STATE == PlayerState.DashingDown)
        {
            ControlDash(Direction.Down);
        }
    }

    void ControlDash(Direction dashDirection = Direction.Up)
    {
        elapsedTime = Time.time - m_timeDashStart;
        
        nextYPos = Mathf.Lerp(m_originalPos.y, finalYPos, elapsedTime / m_dashDuration);
        m_transform.position = new Vector2(m_transform.position.x, nextYPos);
        if (elapsedTime >= m_dashDuration)
        {
            // Stop dashing and come back to idle state
            m_playerScript.ChangePlayerState(PlayerState.Idle);
        }
    }

    public void Exit()
    {
    }
}
