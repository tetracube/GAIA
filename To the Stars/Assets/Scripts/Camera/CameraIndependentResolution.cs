using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// It modifies the ortographic size of the camera 
/// to keep always the same Unity units in width
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraIndependentResolution : MonoBehaviour
{
    public float m_unityUnits = 16.0f; // Unity units for the width
    
    private void Awake()
    {
        SetOrthographicSize();
    }

    private void SetOrthographicSize(Camera cam = null)
    {
        if (cam == null)
        {
            Camera.main.orthographicSize = 1.0f / Camera.main.aspect * m_unityUnits / 2.0f;
        }
        else
        {
            cam.orthographicSize = 1.0f / Camera.main.aspect * m_unityUnits / 2.0f;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        SetOrthographicSize();
    }
#endif

 
}
