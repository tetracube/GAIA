using UnityEngine;
using System.Collections;
using System.IO;
using FSM;

/// <summary>
/// Class honding the Finite State Machines Manager. It holds all the different 
/// State Machines. Since only one instance of this makes sense and different
/// elements could request a State Machine the Singletone Patterns has been implemented
/// to simplify things.
/// </summary>
public class FSM_Controller : MonoBehaviour 
{
    public TextAsset[] m_xmlFilesFSM; // ngs - xml files with the specification of the finite state machines

    public FSM_Manager m_managerFSM;		 //FSM_MANAGER 		-> controls the parsing and initialization of FSMs

    // Singleton
    private static FSM_Controller m_instance;

    public static FSM_Controller INSTANCE
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<FSM_Controller>();
            }
            return m_instance;
        }
    }

	void Awake () 
    {
        //Creation of a new xmltest.FSM_Parser
        xmltest.FSM_Parser parser = new xmltest.FSM_Parser();
     
        //Creation of a new FSM_Manager (with a parser)
        m_managerFSM = new FSM_Manager(parser);

        //Loads and parses all files
        if (m_xmlFilesFSM != null)
        {
            for (int i = 0; i < m_xmlFilesFSM.Length; i++)
            {
                m_managerFSM.addFSM(m_xmlFilesFSM[i].text);
            }
        }

        parser.WriteLog("");
	}
	
}
