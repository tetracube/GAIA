using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class StarFSM : MonoBehaviour 
{
    private bool            m_changeToHappyStar = false; // To register when the star has to change state to Happy
    private bool            m_changeToHitStar   = false; // To register when the star has to change state to Hit

    private SpriteRenderer  m_spriteRenderer;
    private Transform       m_transform;
    private Animator        m_animator;

    // To calculate distance to the player
    private Transform       m_playerTransform;

    // Finite State Machine
    private FSM_Manager     m_managerFSM;
    private FSM_Machine     m_fsmStar;
    List<int>               m_doActions;  // List of actions to do according to the State Machine's state
    List<int>               m_eventsList;

    void Start()
    {
        m_eventsList = new List<int>();

        // Request the finite state machine of the star
        m_managerFSM      = FSM_Controller.INSTANCE.m_managerFSM;
        m_fsmStar         = m_managerFSM.createMachine(this, Tags.CLASSIC, "StarClassicDeterministic");

        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_spriteRenderer  = GetComponent<SpriteRenderer>();
        m_animator        = GetComponent<Animator>();
        m_transform       = transform;
	}

    void Update()
    {
        m_doActions = m_fsmStar.UpdateFSM();

        // Execute the corresponding actions according to the state of the State Machine
        for (int i = 0; i < m_doActions.Count; i++)
        {
            if (i != Tags.UNKNOWN)
            {
                ExecuteAction(m_doActions[i]);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the star was unlit(sad) then lighten it(make it happy) when the character touches it
        if (other.CompareTag("Character") && m_fsmStar.getCurrentState().getTag() == Tags.SAD_STAR)
        {
            m_changeToHappyStar = true;
        }

        // If the star collides with the umbrella
        else if (other.gameObject.layer == 11)
        {
            m_changeToHitStar = true;
        }
    }

    /// <summary>
    /// It is automatically invoked by the FSM
    /// </summary>
    /// <returns></returns>
    public List<int> CheckEvents()
    {
        m_eventsList.Clear();

        // If player touched a star
        if (m_changeToHappyStar)
        {
            m_eventsList.Add(Tags.EVENT_HAPPY_STAR);
            m_changeToHappyStar = false;
        }

        // Check whether the player passed beyond the sad star's position
        if (m_fsmStar.getCurrentState().getTag() == Tags.SAD_STAR &&
            m_playerTransform.position.x > m_transform.position.x + m_spriteRenderer.bounds.size.x)
        {
            m_eventsList.Add(Tags.EVENT_LOST_STAR);
        }

        // Check if the star has been hit by the umbrella
        if (m_changeToHitStar)
        {
            m_eventsList.Add(Tags.EVENT_HIT_STAR);
            m_changeToHitStar = false;
        }

        // Necessary to add the UNKNOWN event if no other was found
        if (m_eventsList.Count == 0)
        {
            m_eventsList.Add(Tags.UNKNOWN);
        }

        return m_eventsList;
    }

    public void ExecuteAction(int action)
    {
        switch (action)
        {
            case Tags.SAD_STAR_ACTION:
                // Action to perform while in SAD_STAR's state
                break;

            case Tags.HAPPY_STAR_ACTION:
                break;

            case Tags.LOST_STAR_ACTION:
                break;

            case Tags.HIT_STAR_ACTION:
                break;

            case Tags.LOST_STAR_IN_ACTION:
                m_animator.SetTrigger("Lost");
                GameManager.INSTANCE.GameOver(GameOverCause.LostStar);
                AudioManager.INSTANCE.PlaySound(AudioManager.Sound.LostStar);
                break;

            case Tags.HAPPY_STAR_IN_ACTION:
                m_animator.SetTrigger("Happy");
                AudioManager.INSTANCE.PlaySound(AudioManager.Sound.LightenStar);
                break;

            case Tags.HIT_STAR_IN_ACTION:
                m_animator.SetTrigger("Hit");
                GameManager.INSTANCE.GameOver(GameOverCause.UmbrellaHitStar);
                AudioManager.INSTANCE.PlaySound(AudioManager.Sound.StarUmbrellaHit);
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
