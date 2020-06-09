using UnityEngine;
using System.Collections;

public class LevelCompletedBehaviour : StateMachineBehaviour
{
    private SMB_Objects m_menuReferences;
    public float        m_fadeInTime = 1.0f;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        Time.timeScale = 0.0f;

        // Get object with references to menu panels
        if (m_menuReferences == null)
        {
            m_menuReferences = GameObject.Find("MenuReferences").GetComponent<SMB_Objects>();
        }

        // Activate LevelCompleted Menu
        m_menuReferences.m_levelCompletedPanel.SetActive(true);

        // Make it appear smootly if possible
        if (m_menuReferences.m_levelCompletedPanel.GetComponent<CanvasGroupFunctions>() != null)
        {
            m_menuReferences.m_levelCompletedPanel.GetComponent<CanvasGroupFunctions>().AppearGroup(m_fadeInTime);
        }

        UnlockNextLevel();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        Time.timeScale = 1;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    void UnlockNextLevel()
    {
        
        string[] stringSeparators = new string[] { "Stage", "Level" };
        string currentLevelName   = Application.loadedLevelName;
        
        // It holds the current stage and level
        string[] levelData        = currentLevelName.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);

        // Unlock next level
        int nextLevel            = int.Parse(levelData[1]) + 1;
        PlayerPrefs.SetInt("Stage" + levelData[0] + "Level" + nextLevel, 1);
    }
}
