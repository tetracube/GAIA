using UnityEngine;

public class WinManager : MonoBehaviour
{
    private const string LOG_FILE = "win.log";

    enum EFightResult
    {
        WIN, DRAW, LOSE
    };

    private int m_Win = 0;
    private int m_Draw = 0;
    private int m_Total = 0;
    private int m_MessagesCount = 0;
    private string m_Messages = "";

    public void AddWin()
    {
        m_Win++;
        m_Total++;
        LogMessage(EFightResult.WIN);
    }

    public void AddDraw()
    {
        m_Draw++;
        m_Total++;
        LogMessage(EFightResult.DRAW);
    }

    public void AddLose()
    {
        m_Total++;
        LogMessage(EFightResult.LOSE);
    }

    public void ResetWins()
    {
        m_Win = 0;
        m_Total = 0;
    }

    private void LogMessage(EFightResult FightResult)
    {
        if (m_Total == m_Draw) { return; }

        m_Messages += "Agent ";

        switch (FightResult)
        {
            case EFightResult.WIN:
                m_Messages += "won";
                break;
            case EFightResult.LOSE:
                m_Messages += "lost";
                break;
            case EFightResult.DRAW:
                m_Messages += "draw";
                break;
        }
        
        m_Messages += ": " + m_Win + " / " + (m_Total - m_Draw) + " = " + ((float)m_Win) / (m_Total - m_Draw) + "\n";

        if (FightResult != EFightResult.DRAW)
        {
            m_MessagesCount++;

            if (m_MessagesCount > 20)
            {
                m_MessagesCount = 0;
                GAIA.Utils.Log(m_Messages, LOG_FILE);
                m_Messages = "";
            }
        }
    }
}
