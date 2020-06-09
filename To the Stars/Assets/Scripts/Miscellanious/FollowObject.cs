using UnityEngine;
using System.Collections;

/// <summary>
/// It follows an object's movement on its x-axis
/// </summary>
public class FollowObject : MonoBehaviour 
{
    public  Transform m_objectToFollow;
    private Vector3   m_prevPosObjectToFollow;
    private Transform m_transform;

	void Start () 
    {
        m_transform = transform;
        m_prevPosObjectToFollow = m_objectToFollow.position;
	}
	
	void Update () 
    {
        // Translate this object on x-axis the distance the followed object moved
        float deltaXPos = m_objectToFollow.position.x - m_prevPosObjectToFollow.x;
        m_transform.Translate(new Vector3(deltaXPos, 0, 0));

        m_prevPosObjectToFollow = m_objectToFollow.position;
	}
}

