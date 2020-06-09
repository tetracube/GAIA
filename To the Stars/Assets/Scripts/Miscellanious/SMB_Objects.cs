using UnityEngine;
using System.Collections;

/// <summary>
/// This Class will hold references to objects for State Machine Behaviour scripts to use.
/// Since a State Machine Behaviour can not store references to game objects. Those references
/// will be given from here. The state machine behaviour just have to find this objecto on the scene
/// </summary>
public class SMB_Objects : MonoBehaviour 
{
    public GameObject m_pausePanel;
    public GameObject m_gameOverPanel;
    public GameObject m_levelCompletedPanel;

    public AudioSource m_musicLevel;
    public AudioSource m_musicGameOver;
    public AudioSource m_musicPause;
}
