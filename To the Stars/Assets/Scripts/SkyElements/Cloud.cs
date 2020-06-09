using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour
{
    private Animator  m_animator;
    private Player    m_playerScript;
    private Transform m_transform;

    void Start()
    {
        m_transform    = transform;
        m_animator     = GetComponent<Animator>();
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If cloud collides with the character
        if (other.CompareTag("Character"))
        {
            m_animator.SetTrigger("CloudStrike");
        }

        // If the cloud collides with the umbrella
        if (other.gameObject.layer == 11)
        {
            // If the cloud is at the same lane of the player's one it means the player hit the cloud from the front
            if (Mathf.Abs(m_playerScript.CURRENT_LANE.transform.position.y - m_transform.position.y) < 0.5f)
            {
                m_animator.SetTrigger("CloudBounce");
                GameManager.INSTANCE.GameOver(GameOverCause.UmbrellaFrontCloud);         
            }
            else
            {
                m_animator.SetTrigger("CloudBounce");

                // To make the player come back to the lane it came from
                m_playerScript.ChangePlayerState(PlayerState.Idle);
            }
        }
    }
}
