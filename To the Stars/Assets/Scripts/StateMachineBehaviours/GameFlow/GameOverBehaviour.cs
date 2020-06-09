using UnityEngine;
using System.Collections;

public class GameOverBehaviour : StateMachineBehaviour 
{
    private SMB_Objects m_menuReferences;
    public float        m_fadeInTime = 1.0f;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Time.timeScale = 0.0f;

        // Get object with references to menu panels
        if (m_menuReferences == null)
        {
            m_menuReferences = GameObject.Find("MenuReferences").GetComponent<SMB_Objects>();
        }

        // Activate GameOver Menu
        m_menuReferences.m_gameOverPanel.SetActive(true);

        // Make it appear smootly if possible
        if (m_menuReferences.m_gameOverPanel.GetComponent<CanvasGroupFunctions>() != null)
        {
            m_menuReferences.m_gameOverPanel.GetComponent<CanvasGroupFunctions>().AppearGroup(m_fadeInTime);
        }

        //Stop InGame music and activate GameOver music
        m_menuReferences.m_musicLevel.Pause();
        m_menuReferences.m_musicGameOver.Play();
	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
	//
	//}

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Time.timeScale = 1;
	}
}
