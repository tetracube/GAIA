using UnityEngine;
using System.Collections;

public class PauseBehaviour : StateMachineBehaviour 
{
    private SMB_Objects m_menuReferences;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Play Button sound
        AudioManager.INSTANCE.PlaySound(AudioManager.Sound.Button);

        Time.timeScale = 0.0f;

        // Get object with references to menu panels
        if (m_menuReferences == null)
        {
            m_menuReferences = GameObject.Find("MenuReferences").GetComponent<SMB_Objects>();
        }

        // Show Pause Menu
        m_menuReferences.m_pausePanel.SetActive(true);

        //Stop InGame music and activate Pause music
        m_menuReferences.m_musicLevel.Pause();
        m_menuReferences.m_musicPause.Play();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_menuReferences != null)
        {
            // Hide Pause Menu
            m_menuReferences.m_pausePanel.SetActive(false);

            // Resume InGame music and stop Pause music
            m_menuReferences.m_musicLevel.UnPause(); ;
            m_menuReferences.m_musicPause.Stop();
        }
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
