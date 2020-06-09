using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    private Transform m_transform;

    public enum Sound
    {
        Button, LightenStar, StarUmbrellaHit, LostStar, UmbrellaOpenSound, UmbrellaCloseSound, CloudBounce, CloudStrike
    }

    #region SOUND_EFFECTS
    public AudioClip   m_buttonSound;
    public AudioClip[] m_lightenStarSound;
    public AudioClip   m_starUmbrellaHit;
    public AudioClip   m_lostStar;
    public AudioClip   m_umbrellaOpenSound;
    public AudioClip   m_umbrellaCloseSound;
    public AudioClip   m_cloudBounceSound;
    public AudioClip   m_cloudStrikeSound;
    #endregion

    // Singleton
    private static AudioManager m_instance;

    #region PROPERTIES
    public static AudioManager INSTANCE
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<AudioManager>();
            }
            return m_instance;
        }
    }
    #endregion

    // Use this for initialization
	void Awake () 
    {
        m_transform = transform;

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

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.Button:
                AudioSource.PlayClipAtPoint(m_buttonSound, m_transform.position);
                break;

            case Sound.LightenStar:
                int randomSoundIndex = Random.Range(0, m_lightenStarSound.Length);
                AudioSource.PlayClipAtPoint(m_lightenStarSound[randomSoundIndex], m_transform.position);
                break;

            case Sound.StarUmbrellaHit:
                AudioSource.PlayClipAtPoint(m_starUmbrellaHit, m_transform.position);
                break;

            case Sound.LostStar:
                AudioSource.PlayClipAtPoint(m_lostStar, m_transform.position);
                break;

            case Sound.UmbrellaOpenSound:
                AudioSource.PlayClipAtPoint(m_umbrellaOpenSound, m_transform.position);
                break;

            case Sound.UmbrellaCloseSound:
                AudioSource.PlayClipAtPoint(m_umbrellaCloseSound, m_transform.position);
                break;

            case Sound.CloudBounce:
                AudioSource.PlayClipAtPoint(m_cloudBounceSound, m_transform.position);
                break;

            case Sound.CloudStrike:
                AudioSource.PlayClipAtPoint(m_cloudStrikeSound, m_transform.position);
                break;

            default:
                break;
        }
    }
}
