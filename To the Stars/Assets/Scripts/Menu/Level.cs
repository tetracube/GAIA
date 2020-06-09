using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Level : MonoBehaviour 
{
    public MenuLevelState m_levelState = MenuLevelState.Locked;

    [Range(1,10)]
    public int m_stage;

    [Range(1, 10)]
    public int m_level;

    public GameObject m_lockIcon;

    private string m_levelName;
    private Button m_button;

	void Awake () 
    {
        m_button = GetComponent<Button>();
        m_levelName = "Stage" + m_stage.ToString() + "Level" + m_level.ToString();

        // If the state of the level is already defined by PlayerPreferences set it as it says
        if (PlayerPrefs.HasKey(m_levelName))
        {
            if (PlayerPrefs.GetInt(m_levelName) == 0)
            {
                m_levelState = MenuLevelState.Locked;
            }
            else if (PlayerPrefs.GetInt(m_levelName) == 1)
            {
                m_levelState = MenuLevelState.Unlocked;
            }
        }

        // Set state
        ChangeState(m_levelState);
	}

    public void ChangeState(MenuLevelState levelState)
    {
        switch (levelState)
        {
            case MenuLevelState.Locked:
                Lock();
                break;

            case MenuLevelState.Unlocked:
                Unlock();
                break;

            default:
                break;
        }
    }

    public void Lock()
    {
        m_levelState = MenuLevelState.Locked;

        // Set the level as locked
        if (!PlayerPrefs.HasKey(m_levelName))
        {
            PlayerPrefs.SetInt(m_levelName, 0);
        }

        // Activate lock icon if there is any
        if (m_lockIcon != null)
        {
            m_lockIcon.SetActive(true);
        }

        // Lock the button interaction
        m_button.interactable = false;

    }

    public void Unlock()
    {
        m_levelState = MenuLevelState.Unlocked;

        // Set the level as unlocked
        if (PlayerPrefs.GetInt(m_levelName) != 1)
        {
            PlayerPrefs.SetInt(m_levelName, 1);
        }

        // Deactivate lock icon if there is any
        if (m_lockIcon != null)
        {
            m_lockIcon.SetActive(false);
        }

        // Unlock the button interaction
        m_button.interactable = true;
    }
}
