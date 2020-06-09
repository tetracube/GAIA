using UnityEngine;
using System.Collections;

public class GameFlowAnimatorTrigger : MonoBehaviour 
{
    private Animator m_gameManagerAnimator;

	void Start () 
    {
        m_gameManagerAnimator = GameObject.Find("GameManager").GetComponent<Animator>();
	}

    public void SetTrigger(string trigger)
    {
        m_gameManagerAnimator.SetTrigger(trigger);
    }

}
