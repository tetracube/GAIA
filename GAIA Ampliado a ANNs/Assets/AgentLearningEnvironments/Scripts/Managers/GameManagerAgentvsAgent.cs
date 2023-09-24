using UnityEngine;

public class GameManagerAgentvsAgent : MonoBehaviour
{
    public GameObject m_TankAgentPrefab;
    public Transform[] m_SpawnPoints;
    public TankManager[] m_Tanks = new TankManager[2];

    private float m_CurrentEpisodeStart = 0f;
    private readonly float m_EpisodeSeconds = 10f;
    private WinManager m_WinManager;

    private void Start()
    {
        m_WinManager = FindFirstObjectByType<WinManager>();
        SpawnTanks();
        StartRound();
    }

    private void SpawnTanks()
    {
        Transform RandomSpawnPoint1 = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length)];
        Transform RandomSpawnPoint2 = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length)];

        m_Tanks[0].m_Instance = Instantiate(m_TankAgentPrefab, RandomSpawnPoint1.position, RandomSpawnPoint1.rotation);
        m_Tanks[0].m_PlayerNumber = 1;
        m_Tanks[0].SetupAgent();

        m_Tanks[1].m_Instance = Instantiate(m_TankAgentPrefab, RandomSpawnPoint2.position, RandomSpawnPoint2.rotation);
        m_Tanks[1].m_PlayerNumber = 2;
        m_Tanks[1].SetupAIFSM();

        m_Tanks[0].m_Instance.GetComponent<TankAgent>().SetEnemy(m_Tanks[1].m_Instance);
        m_Tanks[0].m_Instance.GetComponent<TankAgent>().SetBehavior("TankTrained");
        m_Tanks[1].m_Instance.GetComponent<TankAgent>().SetEnemy(m_Tanks[0].m_Instance);
        m_Tanks[1].m_Instance.GetComponent<TankAgent>().SetBehavior("TankTrained");

    }


    private void StartRound()
    {
        ResetAllTanks();
        m_CurrentEpisodeStart = Time.time;
    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            Transform RandomSpawnPoint = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length)];
            m_Tanks[i].Reset(RandomSpawnPoint);
        }
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

    private void FixedUpdate()
    {
        if (OneTankLeft() || (Time.time - m_CurrentEpisodeStart) > m_EpisodeSeconds)
        {
            if (m_Tanks[0].m_Instance.GetComponent<TankHealth>().GetCurrentHealth() > 0)
            {
                if (m_Tanks[1].m_Instance.GetComponent<TankHealth>().GetCurrentHealth() > 0)
                {
                    m_WinManager.AddDraw();
                }
                else
                {
                    m_WinManager.AddWin();
                }
            }
            else
            {
                m_WinManager.AddLose();
            }

            StartRound();
        }
    }
}