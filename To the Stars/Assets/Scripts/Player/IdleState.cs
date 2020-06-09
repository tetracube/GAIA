using UnityEngine;
using System.Collections;

/// <summary>
/// This state keeps the players always on its lane; Top, Middle, or Bottom.
/// To avoid the player being at the middle of two lanes or somewhere else.
/// </summary>
public class IdleState : MonoBehaviour, IState
{
    private Player    m_playerScript;
    private Transform m_transform;

    public float      m_returnSpeed = 10.0f; // Speed at which the player comes back to its lane, after crashing agains a cloud or something

    void Start()
    {
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_transform    = transform;
    }

    public void Enter()
    {
    }

    public void UpdateState()
    {
        // If the player is not on its lane, come back to it over time.
        if (m_playerScript.transform.position.y != m_playerScript.CURRENT_LANE.transform.position.y)
        {
            float yPos = Mathf.Lerp(m_transform.position.y, m_playerScript.CURRENT_LANE.transform.position.y, Time.deltaTime * m_returnSpeed);
            m_transform.position = new Vector2(m_transform.position.x, yPos);
        }
    }

    public void Exit()
    {
    }
}
