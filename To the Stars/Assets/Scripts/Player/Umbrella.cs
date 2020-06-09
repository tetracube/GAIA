using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Umbrella : MonoBehaviour
{
    private Player          m_playerScript;
    private Transform       m_transform;
    private Animator        m_animator;

    public CircleCollider2D m_characterCollider; // Deactivate character collider while the umbrella is opened

	void Start () 
    {
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_transform    = transform;
        m_animator     = GetComponent<Animator>();
  
        // Subscribe to input manager for handling the umbrella's input
        InputManager.OnInputEvent += HandleInput;

        GameManager.OnGameOver += OnGameOver;
	}

    void Update()
    {
        // Rotate towards moving direction if the player is moving
        if (m_playerScript.MOVEMENT_DIR.magnitude != 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(m_playerScript.MOVEMENT_DIR);
            m_transform.rotation = lookRotation;
            m_transform.Rotate(Vector3.up, -90);
        }
    }

    void HandleInput(InputType input)
    {
        if(input == InputType.OpenUmbrella)
        {
            m_animator.SetBool("Umbrella", true);

            // Reduce character collider to avoid collisions other than the umbrella
            m_characterCollider.enabled = false;

            // Play umbrella open sound
            AudioManager.INSTANCE.PlaySound(AudioManager.Sound.UmbrellaOpenSound);
        }
        else if (input == InputType.CloseUmbrella)
        {
            m_animator.SetBool("Umbrella", false);
            m_characterCollider.enabled = true;

            // Play umbrella close sound
            AudioManager.INSTANCE.PlaySound(AudioManager.Sound.UmbrellaCloseSound);
        }
    }

    void OnDisable()
    {
        InputManager.OnInputEvent -= HandleInput;
        GameManager.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        enabled = false;
    }

}
