using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour 
{
    public  Sprite          m_happyStar;
    public  Sprite          m_sadStar;
    public  Sprite          m_lostStar;

    // Init state of the star
    private  StarState       m_starState = StarState.Sad;

    private SpriteRenderer  m_spriteRenderer;
    private Transform       m_transform;
    private Animator        m_animator;

    // To calculate distance to the player
    private Transform       m_playerTransform;

	void Start () 
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_transform       = transform;
        m_spriteRenderer  = GetComponent<SpriteRenderer>();
        m_animator        = GetComponent<Animator>();
	}

    void Update()
    {
        // Check whether the player passed beyond the sad star's position
        if (m_starState == StarState.Sad && m_playerTransform.position.x > m_transform.position.x + m_spriteRenderer.bounds.size.x)
        {
            ChangeState(StarState.Lost);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the star was unlit(sad) then lighten it(make it happy) when the character touches it
        if(other.CompareTag("Character") && m_starState == StarState.Sad)
        {
            ChangeState(StarState.Happy);
        }
    }

    /// <summary>
    /// // Change the star's state and its sprite according to it
    /// </summary>
    void ChangeState(StarState starState)
    {
        m_starState = starState;

        switch (m_starState)
        {
            case StarState.Sad:
                break;

            case StarState.Happy:
                m_animator.SetTrigger("Happy");
                break;

            case StarState.Lost:
                m_animator.SetTrigger("Lost");
                GameManager.INSTANCE.GameOver(GameOverCause.LostStar);
                break;

            default:
                break;
        }
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }
}
