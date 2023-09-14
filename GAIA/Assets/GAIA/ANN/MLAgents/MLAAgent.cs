using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using GAIA;
using Unity.Barracuda;
using UnityEngine;
using System.IO;
using Unity.Barracuda.ONNX;

public class MLAAgent : Unity.MLAgents.Agent, GAIA.IAgent
{
    private const string AGENT_ONNX_FOLDER = "Assets/GAIA/ANN/MLAgents/Agents/";

    private OnAddObservationsSignature m_OnAddObservation;
    private OnActionsReceivedSignature m_OnActionReceived;

    private VectorSensor m_Sensor;
    private AgentConfig m_Config;
    private bool m_Initialized = false;


    public AgentConfig Config {
        get => m_Config;
        set {
            m_Config = value;
            Configure();
        }
    }

    private void Configure()
    {
        if (Config.AutomaticActionRequester)
        {
            gameObject.AddComponent<DecisionRequester>();
        }

        BehaviorParameters Parameters = GetComponent<BehaviorParameters>();
        Parameters.BehaviorName = Config.Name;
        Parameters.BrainParameters.VectorObservationSize = Config.NumberOfObservations;
        int[] DiscreteBranches = new int[Config.DiscreteActions.Length];
        for (int i = 0; i < DiscreteBranches.Length; i++)
        {
            DiscreteBranches[i] = Config.DiscreteActions[i].To - Config.DiscreteActions[i].From + 1;
        }
        Parameters.BrainParameters.ActionSpec = new ActionSpec(Config.ContinuosActions.Length, DiscreteBranches);
        if (!Config.Train)
        {
            Parameters.Model = LoadModel(Config.Name);
            Parameters.BehaviorType = BehaviorType.InferenceOnly;
        }

        base.OnEnable();
        m_Initialized = true;
    }

    private NNModel LoadModel(string BehaviorName)
    {
        NNModel Model = ScriptableObject.CreateInstance<NNModel>();
        Model.name = Config.Name;
        Model.modelData = LoadModelData();
        return Model;

        NNModelData LoadModelData()
        {
            NNModelData ModelData = ScriptableObject.CreateInstance<NNModelData>();
            Model OnnxModel = new ONNXModelConverter(true).Convert(AGENT_ONNX_FOLDER + BehaviorName + ".onnx");
            using (MemoryStream MemoryStream = new())
            {
                using BinaryWriter Writer = new(MemoryStream);
                ModelWriter.Save(Writer, OnnxModel);
                ModelData.Value = MemoryStream.ToArray();
            }

            return ModelData;
        }
    }

    public void SetOnAddObservations(OnAddObservationsSignature method)
    {
        m_OnAddObservation = method;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        m_Sensor = sensor;
        m_OnAddObservation?.Invoke();
    }

    public void AddObservation(float val)
    {
        m_Sensor.AddObservation(val);
    }

    public void SetOnActionsReceived(OnActionsReceivedSignature method)
    {
        m_OnActionReceived = method;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float[] CActions = ChangeRange(actions.ContinuousActions.Array);
        int[] DActions = ShiftRange(actions.DiscreteActions.Array);

        if (m_OnActionReceived != null)
            m_OnActionReceived(CActions, DActions);
    }

    private float[] ChangeRange(float[] Array)
    {
        float[] RangedActions = new float[Array.Length];

        for (int i = 0; i < Array.Length; ++i)
        {
            float From = Config.ContinuosActions[i].From;
            float To = Config.ContinuosActions[i].To;
            RangedActions[i] = GAIA.Utils.ChangeRange(Array[i], -1f, 1f, From, To);
        }
        return RangedActions;
    }


    private int[] ShiftRange(int[] Array)
    {
        int[] RangedActions = new int[Array.Length];
        for (int i = 0; i < Array.Length; ++i)
        {
            RangedActions[i] = GAIA.Utils.ShiftRange(Array[i], Config.DiscreteActions[i].From);
        }
        return RangedActions;
    }


    public new void RequestAction()
    {
        base.RequestDecision();
    }

    public new void AddReward(float reward)
    {
        base.AddReward(reward);
    }

    public new void SetReward(float reward)
    {
        base.SetReward(reward);
    }

    public new void EndEpisode()
    {
        base.EndEpisode();
    }

    protected override void OnEnable() {
        if (m_Initialized)
        {
            base.OnEnable();
        }
    }
}
