using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceFunctions : MonoBehaviour 
{
    private AudioSource m_audioSource;
	
	void Start () 
    {
        m_audioSource = GetComponent<AudioSource>();
	}

    public void PlayAudio()
    {
        m_audioSource.Play();
    }

    public void PauseAudio()
    {
        m_audioSource.Pause();
    }

    public void UnPuaseAudio()
    {
        m_audioSource.UnPause();
    }
}
