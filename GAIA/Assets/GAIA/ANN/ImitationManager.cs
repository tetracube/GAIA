using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ImitationManager : MonoBehaviour
{
    private const string LOG_FILE = "training.log";
    private const string RESULTS_PATH = "results/";
    private const string CHECKPOINT_EXTENSION = "pt";
    private const float CHECK_FREQUENCY_SECONDS = 1f;

    public string AgentName = "Tank";
    [Range(0.0f, 1.0f)]
    public float ImitationThreshold = 0.05f;
    public int StopCount = 5;

    private string m_LastCheckpoint = string.Empty;
    private float m_BestImitationPercentage = 0f;
    private int m_ImitationBelowBestCount = 0;
    private int m_ImitationCorrect = 0;
    private int m_ImitationTotal = 0;

    void Start()
    {
        GAIA.Utils.Log("Starting training... ", LOG_FILE);
        m_LastCheckpoint = GetNewestCheckpoint();
        StartCoroutine(CheckForNewCheckpoint());
    }

    public void AddImitationSample(int Correct, int Total)
    {
        m_ImitationCorrect += Correct;
        m_ImitationTotal += Total;
}

    private string GetNewestCheckpoint()
    {
        if (!System.IO.Directory.Exists(GetCheckpointsPath())) { return ""; }

        string[] CheckPoints = FilterCheckpoints(System.IO.Directory.GetFiles(GetCheckpointsPath()));
        string NewestCheckpoint = "";
        if (CheckPoints.Length > 0)
        {
            DateTime NewestCheckpointCreationDate = System.IO.File.GetCreationTime(GetCheckpointFilePath(CheckPoints[0]));
            NewestCheckpoint = CheckPoints[0];
            for (int i = 1; i < CheckPoints.Length; i++)
            {
                DateTime FileCreationDate = System.IO.File.GetCreationTime(GetCheckpointFilePath(CheckPoints[i]));
                if (FileCreationDate > NewestCheckpointCreationDate)
                {
                    NewestCheckpointCreationDate = FileCreationDate;
                    NewestCheckpoint = CheckPoints[i];
                }
            }
        }

        return NewestCheckpoint;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetCheckpointsPath()
    {
        return RESULTS_PATH + AgentName + "/" + AgentName + "/";
    }

    private string[] FilterCheckpoints(string[] Files)
    {
        List<string> Checkpoints = new();
        foreach (string File in Files)
        {
            string FileName = System.IO.Path.GetFileName(File);
            if (IsCheckpoint(FileName))
            {
                Checkpoints.Add(FileName);
            }
        }
        return Checkpoints.ToArray();
    }

    private string GetCheckpointFilePath(string CheckpointFileName)
    {
        return GetCheckpointsPath() + CheckpointFileName;
    }

    private bool IsCheckpoint(string FileName)
    {
        return !FileName.StartsWith("checkpoint") && FileName.EndsWith("." + CHECKPOINT_EXTENSION);
    }

    private IEnumerator CheckForNewCheckpoint()
    {
        string NewestCheckpoint = GetNewestCheckpoint();
        if (NewestCheckpoint != m_LastCheckpoint)
        {
            m_LastCheckpoint = NewestCheckpoint;
            CheckImitationPercentage();
            ResetImitationPercentage();
        }

        yield return new WaitForSeconds(CHECK_FREQUENCY_SECONDS * (Time.timeScale / 4));
        StartCoroutine(CheckForNewCheckpoint());
    }

    private void CheckImitationPercentage()
    {
        float Percentage = ((float)m_ImitationCorrect) / m_ImitationTotal;
        GAIA.Utils.Log("Imitation percentage of checkpoint " + m_LastCheckpoint + ": " + Percentage, LOG_FILE);

        if (Percentage > ImitationThreshold && StopCount == 0)
        {
            GAIA.Utils.Log("Imitation threshold reached: " + ImitationThreshold, LOG_FILE);
            GAIA.Utils.Log("Stopping training...", LOG_FILE);
            StopTraining();
        }

        if (Percentage > m_BestImitationPercentage)
        {
            m_BestImitationPercentage = Percentage;
            m_ImitationBelowBestCount = 0;
        }
        else if (m_BestImitationPercentage > ImitationThreshold)
        {
            m_ImitationBelowBestCount++;
            if (m_ImitationBelowBestCount >= StopCount)
            {
                GAIA.Utils.Log("No improvement in " + m_ImitationBelowBestCount + " checkpoints", LOG_FILE);
                GAIA.Utils.Log("Stopping training...", LOG_FILE);
                StopTraining();
            }
        }
    }

    private void ResetImitationPercentage()
    {
        m_ImitationCorrect = 0;
        m_ImitationTotal = 0;
    }

    private void StopTraining()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
