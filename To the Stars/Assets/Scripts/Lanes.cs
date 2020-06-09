using UnityEngine;
using System.Collections;

public class Lanes : MonoBehaviour 
{
    public Lane m_topLane;
    public Lane m_middleLane;
    public Lane m_bottomLane;

    // Singleton
    private static Lanes m_instance;

    #region PROPERTIES
    public static Lanes INSTANCE
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<Lanes>();
            }
            return m_instance;
        }
    }
    #endregion

    void Awake()
    {
        #region SINGLETON
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            if (this != m_instance)
            {
                Destroy(this.gameObject);
            }
        }
        #endregion
    } 
}
