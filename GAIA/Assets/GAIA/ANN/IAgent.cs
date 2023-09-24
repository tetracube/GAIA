using static MLAAgent;

namespace GAIA
{
    public delegate void OnAddObservationsSignature();
    public delegate void OnActionsReceivedSignature(float[] ContinuousActions, int[] DiscreteActions);

    public struct DiscreteAction
    {
        public int From;
        public int To;
    }

    public struct ContinuousAction
    {
        public float From;
        public float To;
    }

    public struct AgentConfig
    {
        public string Name;
        public string Type;
        public int NumberOfObservations;
        public bool Train;
        public bool ShowImitationPercentage;
        public bool AutomaticActionRequester;
        public DiscreteAction[] DiscreteActions;
        public ContinuousAction[] ContinuosActions;
        public EnvironmentConfig EnvironmentConfig;
    }

    public interface IAgent
    {
        AgentConfig Config { get; set; }

        static public AgentConfig DefaultConfig()
        {
            AgentConfig config = new()
            {
                Name = "Agent",
                Type = "MLAgent",
                NumberOfObservations = 1,
                Train = false,
                ShowImitationPercentage = true,
                DiscreteActions = new DiscreteAction[0],
                ContinuosActions = new ContinuousAction[1] { new ContinuousAction { From = -1f, To = 1f } },
                AutomaticActionRequester = false,
                EnvironmentConfig = IEnvironment.DefaultConfig()
            };
            return config;
        }

        void SetOnAddObservations(OnAddObservationsSignature method);
        void SetOnActionsReceived(OnActionsReceivedSignature method);

        void AddObservation(float val);
        void RequestAction();

        void AddReward(float reward);
        void SetReward(float reward);

        void EndEpisode();
    }
}
