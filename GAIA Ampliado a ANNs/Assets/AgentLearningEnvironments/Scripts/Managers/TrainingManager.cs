using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    private const string NETWORKS_PATH = "results/Tank/Tank";
    private const string NETWORK_EXTENSION = "onnx";

    public List<float> CheckpointMinutes = new(new float[] { 60f, 120f, 240f, 480f });

    private int m_NextCheckpointIndex = 0;

    void Start()
    {
        StartCoroutine(SafeNeuralNetworkState());
    }

    private IEnumerator SafeNeuralNetworkState()
    {
        float WaitSeconds = CheckpointMinutes[m_NextCheckpointIndex] * 60f - Time.unscaledTime;
        yield return new WaitForSecondsRealtime(WaitSeconds);
        string LastNeuralNetwork = GetNewestNeuralNetwork();
        string NetworkPath = NETWORKS_PATH + "/" + LastNeuralNetwork;
        System.IO.File.Copy(NetworkPath, NETWORKS_PATH + "/" + "Tank_" + m_NextCheckpointIndex + "." + NETWORK_EXTENSION);
        ++m_NextCheckpointIndex;

        if (m_NextCheckpointIndex < CheckpointMinutes.Count)
        {
            StartCoroutine(SafeNeuralNetworkState());
        }
    }

    private string GetNewestNeuralNetwork()
    {
        if (!System.IO.Directory.Exists(NETWORKS_PATH)) { return ""; }

        string[] NeuralNetworks = FilterNeuralNetworks(System.IO.Directory.GetFiles(NETWORKS_PATH));
        string NewestCheckpoint = "";
        if (NeuralNetworks.Length > 0)
        {
            System.DateTime NewestCheckpointCreationDate = System.IO.File.GetCreationTime(GetCheckpointPath(NeuralNetworks[0]));
            NewestCheckpoint = NeuralNetworks[0];
            for (int i = 1; i < NeuralNetworks.Length; i++)
            {
                System.DateTime FileCreationDate = System.IO.File.GetCreationTime(GetCheckpointPath(NeuralNetworks[i]));
                if (FileCreationDate > NewestCheckpointCreationDate)
                {
                    NewestCheckpointCreationDate = FileCreationDate;
                    NewestCheckpoint = NeuralNetworks[i];
                }
            }
        }

        return NewestCheckpoint;
    }

    private string[] FilterNeuralNetworks(string[] Files)
    {
        List<string> NeuralNetworks = new();
        foreach (string File in Files)
        {
            string FileName = System.IO.Path.GetFileName(File);
            if (IsNeuralNetwork(FileName))
            {
                NeuralNetworks.Add(FileName);
            }
        }
        return NeuralNetworks.ToArray();
    }

    private string GetCheckpointPath(string CheckpointFileName)
    {
        return NETWORKS_PATH + "/" + CheckpointFileName;
    }

    private bool IsNeuralNetwork(string FileName)
    {
        return FileName.EndsWith("." + NETWORK_EXTENSION);
    }
}
