using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    private Animator m_animator;
	void Start () 
    {
        m_animator = GetComponent<Animator>();

        // Subscribe to GameOver event
        GameManager.OnGameOver += OnGameOver;
	}

    void OnGameOver()
    {
        // Set triggers from animator according to the cause of end of game
        switch (GameManager.INSTANCE.GAME_OVER_CAUSE)
        {
            case GameOverCause.CloudStrike:
                m_animator.SetTrigger("GameOverCloudStrike");
                break;
            case GameOverCause.UmbrellaFrontCloud:
                Debug.Log("Umbrella on Cloud");
                m_animator.SetTrigger("GameOverFrontCloud");
                break;
            case GameOverCause.UmbrellaHitStar:
                m_animator.SetTrigger("GameOverHitStar");
                break;
            case GameOverCause.LostStar:
                m_animator.SetTrigger("GameOverLostStar");
                break;
            default:
                break;
        }
    }

    void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }
}
