using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// It detects finger touch for all the defined cameras
/// </summary>
public class InputManager : MonoBehaviour 
{
    public Camera[]   m_cameras;
    public float      m_dragMinDistance = 0.5f; // Minimum distance 
    private Vector3   m_dragInitPos;            // Position where the drag started

    // Event called when input is detected
    public delegate void InputEvent(InputType type);
    public static event InputEvent OnInputEvent;

    // IDs of the fingers tapping the left side of the screen
    List<int> m_umbrellaFingersID;

    private Player m_playerScript;

    void Start()
    {
        m_umbrellaFingersID = new List<int>();
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        // If there are not listeners to the input, return
        if (OnInputEvent == null)
        {
            return;
        }

        #region DEBUG_INPUT
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Jump Down
            OnInputEvent(InputType.JumpDown);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            // Jump Up
            OnInputEvent(InputType.JumpUp);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Dash Down
            OnInputEvent(InputType.DashDown);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Dash Up
            OnInputEvent(InputType.DashUp);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            OnInputEvent(InputType.OpenUmbrella);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
              OnInputEvent(InputType.CloseUmbrella);
        }
        #endregion


		if(Input.touchCount == 0)
		{
			return; 
		}

        Vector3      viewportPointFinger;
        Vector3      viewportPointPlayer;
        Touch        touch;
        for (int i = 0; i < m_cameras.Length; i++)
        {
            for (int j = 0; j < Input.touchCount; j++)
            {
				if (m_cameras[i] == null && !m_cameras[i].enabled)
                {
                    break;
                }

                touch = Input.GetTouch(j);
                viewportPointFinger = m_cameras[i].ScreenToViewportPoint(touch.position);
                viewportPointPlayer = m_cameras[i].WorldToViewportPoint(m_playerScript.transform.position);
                
                // Touch on left side of the screen
                if (viewportPointFinger.x < 0.5f)
                {
                    DetectUmbrella(touch);
                }
                // Touch on right side
                else if (viewportPointFinger.x > 0.5f)
                {
                    DetectPlayerMovement(touch, viewportPointFinger, viewportPointPlayer);
                }
            }
        }
    }

    void DetectUmbrella(Touch touch)
    {
        // If finger just started tapping
        if (touch.phase == TouchPhase.Began)
        {
            // Keep track of the finger and open the umbrella
            m_umbrellaFingersID.Add(touch.fingerId);
            OnInputEvent(InputType.OpenUmbrella);
        }
        // If finger lifted from the screen
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
           // Stop keeping track of the finger
            m_umbrellaFingersID.Remove(touch.fingerId);

            // Close the umbrella if no more fingers are touching
            if (m_umbrellaFingersID.Count == 0)
            {
                OnInputEvent(InputType.CloseUmbrella);
            }
        }
    }

    void DetectPlayerMovement(Touch touch, Vector3 fingerPos, Vector3 playerPos)
    {
        if (touch.phase == TouchPhase.Began)
        {
            m_dragInitPos = touch.position;
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            Vector3 dragFinalPos = touch.position;
            float dragDistance = Vector3.Distance(dragFinalPos, m_dragInitPos);

            if (dragDistance >= m_dragMinDistance)
            {
                // Jump
                if (dragFinalPos.y >= m_dragInitPos.y)
                {
                    OnInputEvent(InputType.JumpUp);
                }
                else
                {
                    OnInputEvent(InputType.JumpDown);
                }

            }
            else
            {
                // Dash
                if (fingerPos.y > playerPos.y)
                {
                    OnInputEvent(InputType.DashUp);
                }
                else
                {
                    OnInputEvent(InputType.DashDown);
                }
            }
       
        }
    }

}
