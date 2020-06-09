using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour 
{
    public AudioSource m_musicLevel;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Pause Music
        m_musicLevel.Stop();

        string completedLevelName = Application.loadedLevelName;
        GameManager.INSTANCE.LevelCompleted(completedLevelName);
    }
}
