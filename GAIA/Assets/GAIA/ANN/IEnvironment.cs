namespace GAIA
{
    public enum EAlgorithm
    {
        PPO
    }

    public struct Memory
    {
        public int Size;
        public int SequenceLength;
    }

    public struct Network
    {
        public int HiddenLayers;
        public int NeuronsPerLayer;
        public Memory Memory;
    }

    public struct ExtrinsicRewards
    {
        public float Strength;
        public float Gamma;
    }

    public struct Curiosity
    {
        public float Strength;
        public float Gamma;
        public float LearningRate;
        public Network Network;
    }

    public struct EnvironmentConfig
    {
        public string Name;
        public EAlgorithm Algorithm;
        public float LearningRate;
        public int BatchSize;
        public bool Resume;
        public Network Network;
        public ExtrinsicRewards ExtrinsicRewards;
        public Curiosity Curiosity;
    }

    public interface IEnvironment
    {
        static public EnvironmentConfig DefaultConfig()
        {
            EnvironmentConfig Config = new()
            {
                Name = "Agent",
                Algorithm = EAlgorithm.PPO,
                LearningRate = 0.0003f,
                BatchSize = 1024,
                Resume = false,
                Network = new Network()
                {
                    HiddenLayers = 2,
                    NeuronsPerLayer = 128,
                    Memory = new Memory() { SequenceLength = 0, Size = 0 }
                },
                ExtrinsicRewards = new ExtrinsicRewards()
                {
                    Strength = 0.3f,
                    Gamma = 0.5f
                },
                Curiosity = new Curiosity()
                {
                    Strength = 0.3f,
                    Gamma = 0.5f,
                    LearningRate = 0.0003f,
                    Network = new Network()
                    {
                        HiddenLayers = 2,
                        NeuronsPerLayer = 128,
                        Memory = new Memory() { SequenceLength = 0, Size = 0 }
                    }
                }
            };
            return Config;
        }

        void AddConfig(EnvironmentConfig Config);
        void Start();
    }
}
