using GAIA;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

public class MLAEnvironment : IEnvironment
{
    private const string SCRIPTS_PATH = "Assets/GAIA/ANN/MLAgents/Scripts/";
    private const string CONFIG_FILE_PATH = "Assets/GAIA/ANN/MLAgents/EnvConfigs/env_config.yaml";
    private const string LOG_FILE = "environment.log";

    private readonly List<EnvironmentConfig> EnvironmentConfigs = new();
    private string ModelName = null;
    private bool Resume = false;

    public void AddConfig(EnvironmentConfig Config)
    {
        EnvironmentConfigs.Add(Config);
        ModelName = Config.Name;
        Resume = Config.Resume;
        string Content = ConfigToStr();
        WriteContentToFile();

        string ConfigToStr()
        {
            string FileContent = "";

            if (EnvironmentConfigs.Count == 1)
            {
                FileContent = "behaviors:" + "\n";
            }
            FileContent += "  " + Config.Name + ":" + "\n";
            FileContent += "    trainer_type: " + Config.Algorithm.ToString().ToLower() + "\n";
            FileContent += "    summary_freq: 10000\n";
            FileContent += "    checkpoint_interval: 10000\n";
            FileContent += "    max_steps: 10000000\n";
            FileContent += "    threaded: true\n";
            FileContent += "    hyperparameters:" + "\n";
            FileContent += "      batch_size: " + Config.BatchSize + "\n";
            FileContent += "      learning_rate: " + Config.LearningRate.ToString(CultureInfo.InvariantCulture) + "\n";
            FileContent += "    network_settings:" + "\n";
            FileContent += "      num_layers: " + Config.Network.HiddenLayers + "\n";
            FileContent += "      hidden_units: " + Config.Network.NeuronsPerLayer + "\n";
            if (Config.Network.Memory.Size > 0)
            {
                FileContent += "      memory:" + "\n";
                FileContent += "        sequence_length: " + Config.Network.Memory.SequenceLength + "\n";
                FileContent += "        memory_size: " + Config.Network.Memory.Size + "\n";
            }
            FileContent += "    reward_signals:" + "\n";
            FileContent += "      extrinsic:" + "\n";
            FileContent += "        strength: " + Config.ExtrinsicRewards.Strength.ToString(CultureInfo.InvariantCulture) + "\n";
            FileContent += "        gamma: " + Config.ExtrinsicRewards.Gamma.ToString(CultureInfo.InvariantCulture) + "\n";
            if (Config.Curiosity.Strength > 0)
            {
                FileContent += "      curiosity:" + "\n";
                FileContent += "        strength: " + Config.Curiosity.Strength.ToString(CultureInfo.InvariantCulture) + "\n";
                FileContent += "        gamma: " + Config.Curiosity.Gamma.ToString(CultureInfo.InvariantCulture) + "\n";
                FileContent += "        learning_rate: " + Config.Curiosity.LearningRate.ToString(CultureInfo.InvariantCulture) + "\n";
                if (Config.Curiosity.Network.Memory.Size > 0)
                {
                    FileContent += "      memory:" + "\n";
                    FileContent += "        sequence_length: " + Config.Curiosity.Network.Memory.SequenceLength + "\n";
                    FileContent += "        memory_size: " + Config.Curiosity.Network.Memory.Size + "\n";
                }
            }

            return FileContent;
        }

        void WriteContentToFile()
        {
            if (EnvironmentConfigs.Count == 1)
            {
                File.WriteAllText(CONFIG_FILE_PATH, Content);
            }
            else
            {
                File.AppendAllText(CONFIG_FILE_PATH, Content);
            }
        }
    }

    public void Start()
    {
        GAIA.Utils.Log("Executing learn.bat", LOG_FILE);
        string ScriptsPath = Path.GetFullPath(SCRIPTS_PATH);

        if (Resume)
        {
            UnityEngine.Debug.Log("Resume");
            CommandExecuter.ExecuteNewWindowSleep(ScriptsPath + "learn.bat " + ModelName + " resume" , 6000);
        }
        else
        {
            CommandExecuter.ExecuteNewWindowSleep(ScriptsPath + "learn.bat " + ModelName, 6000);
        }
    }
}
