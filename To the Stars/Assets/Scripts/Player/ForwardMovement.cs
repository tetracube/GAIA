using UnityEngine;
using System.Collections;

/// <summary>
/// It moves the player towards the right, at a speed which allows 
/// traversing a specified distance when jumping
/// </summary>
[RequireComponent (typeof(JumpState))]
public class ForwardMovement : MonoBehaviour 
{
    public  float       m_jumpDistance = 5.0f;
    
    private Rigidbody2D m_rigidbody2D;
    private float       m_speed;

	void Awake ()
    {
        // Calculate the necessary speed to traverse the specified distance when jumping
        float jumpTime = GetComponent<JumpState>().m_jumpDuration;
        m_speed = m_jumpDistance / jumpTime;

        // Set rigidbody's velocity
        m_rigidbody2D = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        transform.Translate(Vector3.right * m_speed * Time.deltaTime);
    }
}
