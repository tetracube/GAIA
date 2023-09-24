using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;         
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     


    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private NavMeshMovementBT m_AIMovement;
    private NavMeshMovementFSM m_AIMovementFSM;
    private TankAgent m_AIMovementAgent;
    private GameObject m_CanvasGameObject;

    //Setup for playable character
    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";
        SetupCommon();
    }

    //Setup for NPC Enemy
    public void SetupAI()
    {
        m_AIMovement = m_Instance.GetComponent<NavMeshMovementBT>();
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">NPC " + m_PlayerNumber + "</color>";
        SetupCommon();
    }

    public void SetupAIFSM()
    {
        m_AIMovementFSM = m_Instance.GetComponent<NavMeshMovementFSM>();
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">NPC " + m_PlayerNumber + "</color>";
        SetupCommon();
    }

    public void SetupAIFSMTraining()
    {
        m_Instance.GetComponent<TankFSMTraining>().m_PlayerNumber = m_PlayerNumber;
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">NPC " + m_PlayerNumber + "</color>";
        SetupCommon();
    }

    public void SetupAgent()
    {
        m_AIMovementAgent = m_Instance.GetComponent<TankAgent>();
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">NPC " + m_PlayerNumber + "</color>";
        SetupCommon();
    }

    public void SetupCommon()
    {
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }

    public void DisableAIControl()
    {
        m_AIMovement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }

    public void DisableAIControlFSM()
    {
        m_AIMovementFSM.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }

    public void DisableAIControlAgent()
    {
        m_AIMovementAgent.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }

    public void EnableAIControl()
    {
        m_AIMovement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }

    public void EnableAIControlFSM()
    {
        m_AIMovementFSM.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }

    public void EnableAIControlAgent()
    {
        m_AIMovementAgent.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    public void Reset(Transform SpawnPoint)
    {
        m_Instance.transform.position = SpawnPoint.position;
        m_Instance.transform.rotation = SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
