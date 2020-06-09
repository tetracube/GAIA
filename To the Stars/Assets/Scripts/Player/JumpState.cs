using UnityEngine;
using System.Collections;

public class JumpState : MonoBehaviour, IState 
{
    public AnimationCurve m_jumpCurve;

    private float         m_timeJumpStart;
    public  float         m_jumpDuration;

    private Vector3       m_originalPos;
    private Transform     m_transform;

    private Player        m_playerScript;

	void Start () 
    {
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_transform    = transform;
	}

    public void Enter()
    {
        m_timeJumpStart = Time.time;
        m_originalPos = m_transform.position;
    }

    public void UpdateState()
    {
        if (m_playerScript.PLAYER_STATE == PlayerState.JumpingUp)
        {
            ControlJump(Direction.Up);
        }
        else if (m_playerScript.PLAYER_STATE == PlayerState.JumpingDown)
        {
            ControlJump(Direction.Down);
        }
    }

    void ControlJump(Direction jumpDirection = Direction.Up)
    {
        float elapsedTime = Time.time - m_timeJumpStart;
        
        // Jump according to the animation curve
        if (elapsedTime < m_jumpDuration)
        {
            float yPos = m_jumpCurve.Evaluate(elapsedTime / m_jumpDuration) * (int)jumpDirection;
            m_transform.position = new Vector3(m_transform.position.x, m_originalPos.y + yPos, m_transform.position.z);
        }
        else
        {
            // Once the jump has finished come back to original position and state
            m_transform.position = new Vector3(m_transform.position.x, m_originalPos.y, m_transform.position.z);
            m_playerScript.ChangePlayerState(PlayerState.Idle);
        }

        elapsedTime = Time.time - m_timeJumpStart;
    }

    public void Exit()
    {
    }
}
