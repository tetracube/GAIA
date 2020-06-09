using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private Animator      m_animator;
    private GameOverCause m_gameOverCause;
 
    // EVENTS
    public delegate void GameOverEvent();
    public static event GameOverEvent OnGameOver;

    // Singleton
    private static GameManager m_instance;

    #region PROPERTIES
    public static GameManager INSTANCE
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    public GameOverCause GAME_OVER_CAUSE
    {
        get
        {
            return m_gameOverCause;
        }
        set
        {
            m_gameOverCause = value;
        }
    }
    #endregion

    void Awake()
    {
		Application.targetFrameRate = 60;

        m_animator = GetComponent<Animator>();

        #region SINGLETON
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            if (this != m_instance)
            {
                Time.timeScale = 1;
                Destroy(this.gameObject);
            }
        }
        #endregion
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver(GameOverCause cause)
	{
        m_gameOverCause = cause;
        m_animator.SetTrigger("GameOver");

        // Invoke functions subcribed to OnGameOver event
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    public void LevelCompleted(string levelLame)
    {
        m_animator.SetTrigger("LevelCompleted");
    }
}
