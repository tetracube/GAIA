using UnityEngine;
using System.Collections;

public class LevelLockSystem : MonoBehaviour 
{
    [System.Serializable]
    public class Stage
    {
        public Level[] m_levels;
    }

    public Stage[] m_stages;

    /// <summary>
    /// It goes through all the levels and locks them if are not unlocked yet
    /// </summary>
    public void LockLevels()
    {
        int    stageIndex;
        int    levelIndex;
        string levelName;
        
        for (int i = 0; i < m_stages.Length; i++)
        {
            for (int j = 0; j < m_stages[j].m_levels.Length; j++)
            {
                stageIndex = i + 1;
                levelIndex = j + 1;
                levelName = "Stage" + stageIndex.ToString() + "Level" + levelIndex.ToString();
                
                // Lock the new levels and give them a key
                if (!PlayerPrefs.HasKey(levelName))
                {
                    // Lock them setting its key to 0
                    PlayerPrefs.SetInt(levelName, 0);

                    m_stages[i].m_levels[j].Lock();
                }
            }
        }
    }

    /// <summary>
    /// It Check if there is any level unlocked and
    /// unlocks them if that's the case
    /// </summary>
    public void CheckUnlockedLevels()
    {
        int    stageIndex;
        int    levelIndex;
        string levelName;
        
        for (int i = 0; i < m_stages.Length; i++)
        {
            for (int j = 0; j < m_stages[j].m_levels.Length; j++)
            {
                stageIndex = i + 1;
                levelIndex = j + 1;
                levelName = "Stage" + stageIndex.ToString() + "Level" + levelIndex.ToString();
        
                // If the  the new level is unlocked, unlock it 
                if (PlayerPrefs.GetInt(levelName) == 1)
                {
                    // Unlock them setting its key to 1
                    PlayerPrefs.SetInt(levelName, 1);

                    m_stages[i].m_levels[j].Unlock();
                }
            }
        }
    }
}
