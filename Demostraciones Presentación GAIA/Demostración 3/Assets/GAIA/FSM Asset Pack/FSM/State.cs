using System.Collections.Generic;
using System;

namespace GAIA{

    // <summary>
    // State object class
    // </summary>
    // <remarks></remarks>
    public class State {
	
	    //State Class attributes
        //<summary>State tag identifier</summary>
	    int state_TAG;
        //<summary>Action tag identifier of this State</summary>
        int action_TAG;
        //<summary>IN Action tag identifier of this State </summary>
        int in_action_TAG;
        //<summary>OUT Action tag identifier of this State </summary>
        int out_action_TAG;
        //<summary>Flag that determines if this state is initial </summary>
	    string initial;
        //<summary>State's name (string identifier)	 </summary>								
	    string stateName;
        //<summary>List of this state's transitions</summary>	
	    protected List<Transition> stateTransitions;

	    //Class attributes for hierarchical aspect in FAs
        //<summary>Object to attach a submachine</summary>
	    protected FA_Classic subFA;

	    //Class attributes for STACK_BASED FA
        //<summary>Priority of this state. Only used in FA_Stack</summary>
	    protected uint priority;

	    //Class attributes for INERTIAL FA
        //<summary>Latency of this state. Only used in FA_Inertial </summary>
	    protected int latency;
		
	    //Class attributes for CONCURRENT STATES FA (based on Petri's net)
        //<summary>State execution credits. Only used in FA_Concurrent_States </summary>
	    protected short State_credits;
	
	    //Create a new State
	    //Constructor for a classic fsm state
        // <summary>
        // Initializes a new instance of the <see cref="T:FSM.State">State</see> class. 
        // </summary>
        // <param name="ID">Name of this State</param>
        // <param name="FlagInitial">Flag that determines if it is initial ("YES") or it is not ("NO")</param>
        // <param name="state_tag">State tag identifier</param>
        // <param name="action_tag">Action tag identifier of this state</param>
        // <param name="in_action_TAG">IN action tag identifier of this state</param>
        // <param name="out_action_TAG">OUT action tag identifier of this state</param>
        // <remarks>It can be used in every FA</remarks>
	    public State(string ID, string FlagInitial, int state_tag, int action_tag, int in_action_TAG, int out_action_TAG){
		    this.stateName = ID;
		    this.initial = FlagInitial;
		    this.stateTransitions = new List<Transition>();
		    this.state_TAG = state_tag;
		    this.action_TAG = action_tag;
		    this.in_action_TAG = in_action_TAG;
		    this.out_action_TAG = out_action_TAG;
	    }

	    //Constructor for a stack_based fsm state
        // <summary>
        // Initializes a new instance of the <see cref="T:FSM.State">State</see> class. 
        // </summary>
        // <param name="ID">Name of this State</param>
        // <param name="FlagInitial">Flag that determines if it is initial ("YES") or it is not ("NO")</param>
        // <param name="state_tag">State tag identifier</param>
        // <param name="action_tag">Action tag identifier of this state</param>
        // <param name="in_action_TAG">IN action tag identifier of this state</param>
        // <param name="out_action_TAG">OUT action tag identifier of this state</param>
        // <param name="priority">Priority of this State. It has to be >=0 </param>
        // <remarks>Only used in FA_Stack</remarks>
	    public State(string ID, string FlagInitial, int state_tag, int action_tag, int in_action_TAG, int out_action_TAG, uint priority){
		    this.stateName = ID;
		    this.initial = FlagInitial;
		    this.stateTransitions = new List<Transition>();
		    this.state_TAG = state_tag;
		    this.action_TAG = action_tag;
		    this.in_action_TAG = in_action_TAG;
		    this.out_action_TAG = out_action_TAG;
		    this.priority = priority;
	    }
	    //Constructor of an inertial fsm state
        // <summary>
        // Initializes a new instance of the <see cref="T:FSM.State">State</see> class. 
        // </summary>
        // <param name="ID">Name of this State</param>
        // <param name="FlagInitial">Flag that determines if it is initial ("YES") or it is not ("NO")</param>
        // <param name="state_tag">State tag identifier</param>
        // <param name="action_tag">Action tag identifier of this state</param>
        // <param name="in_action_TAG">IN action tag identifier of this state</param>
        // <param name="out_action_TAG">OUT action tag identifier of this state</param>
        // <param name="latency">Latency of this state</param>
        // <remarks>Only used in FA_Inertial</remarks>
	    public State(string ID, string FlagInitial, int state_tag, int action_tag, int in_action_TAG, int out_action_TAG, int latency){
		    this.stateName = ID;
		    this.initial = FlagInitial;
		    this.stateTransitions = new List<Transition>();
		    this.state_TAG = state_tag;
		    this.action_TAG = action_tag;
		    this.in_action_TAG = in_action_TAG;
		    this.out_action_TAG = out_action_TAG;
		    this.latency = latency;
	    }
	    //Constructor of a Concurrent-states fsm state
        // <summary>
        // Initializes a new instance of the <see cref="T:FSM.State">State</see> class. 
        // </summary>
        // <param name="ID">Name of this State</param>
        // <param name="FlagInitial">Flag that determines if it is initial ("YES") or it is not ("NO")</param>
        // <param name="state_tag">State tag identifier</param>
        // <param name="action_tag">Action tag identifier of this state</param>
        // <param name="in_action_TAG">IN action tag identifier of this state</param>
        // <param name="out_action_TAG">OUT action tag identifier of this state</param>
        // <param name="credits">Credits of execution of this state</param>
        // <remarks>Only used in FA_Concurrent_States</remarks>
	    public State(string ID, string FlagInitial, int state_tag, int action_tag, int in_action_TAG, int out_action_TAG, short credits){
		    this.stateName = ID;
		    this.initial = FlagInitial;
		    this.stateTransitions = new List<Transition>();
		    this.state_TAG = state_tag;
		    this.action_TAG = action_tag;
		    this.in_action_TAG = in_action_TAG;
		    this.out_action_TAG = out_action_TAG;
		    this.State_credits = credits;
	    }
	
	    #region ADD METHODS
	
        // <summary>
        // Adds a submachine to this State or changes the one that is already attached 
        // </summary>
        // <param name="subFA"> FA_Classic object (any FA is valid)</param>
        // <returns>
        // 1 if OK
        //-1 if cannot be attached
        //</returns>
        // <remarks></remarks>
	    public int addFA(FA_Classic subFA){
		    try{
			    this.subFA = subFA;
			    return 1;
		    }catch(Exception e){
			    return -1;
		    }
	    }
	
	
        // <summary>
        // Adds a transition to this state if It is possible
        // </summary>
        // <param name="newTransition">Transition object</param>
        // <returns>
        // 1 if OK
        //-1 if error. (Transition has already been added to this State or wrong parameter)
        //</returns>
        // <remarks></remarks>
	    public int addTransition(Transition newTransition){
		    if(stateTransitions.Contains(newTransition)){
			    return -1;
			    //Debug.LogError("State.cs - This state has already the transition");
		    }else{
			    stateTransitions.Add(newTransition);
			    return 1;
		    }
	    }
	    #endregion

	    #region GET METHODS

	
        // <summary>
        // Get the tag assigned to this state
        // </summary>
        // <returns>int value</returns>
        // <remarks></remarks>
	    public int getTag(){
		    return this.state_TAG;
	    }
	
        // <summary>
        // Get the ID that identifies this state
        // </summary>
        // <returns>string value</returns>
        // <remarks></remarks>
	    public string getID(){
		    return this.stateName;
	    }

	
        // <summary>
        // Get latency if it exists
        // </summary>
        // <returns>
        // Latency value if OK
        // -1 if error. This State has not got latency
        //</returns>
        // <remarks></remarks>
	    public int getLatency(){
		    if(this.latency!=null) return this.latency;
		    else return -1;
	    }
	
        // <summary>
        // Get priority if it exists
        // </summary>
        // <returns>
        // Priority value if OK
        // -1 if error. This State has not got priority
        //</returns>
        // <remarks></remarks>
	    public int getPriority(){
		    if(this.priority!=null) return (int)this.priority;
		    else return -1;
	    }

	

        // <summary>
        // Get credits if they exist
        // </summary>
        // <returns>
        // Credits value if OK
        // -1 if error. This State has not got execution credits
        //</returns>
        // <remarks></remarks>
	    public short getCredits(){
		    if(this.State_credits!=null) return this.State_credits;
		    else return -1;
	    }

	
        // <summary>
        // Get subFA if it exists
        // </summary>
        // <returns>
        // SubFA value if OK
        // Null value if error. There is not a subFA attached to this State
        //</returns>
        // <remarks></remarks>
	    public FA_Classic getSubFA(){
		    if(this.subFA!=null) return this.subFA;
		    else return null;
	    }

	
        // <summary>
        // Get if this State is initial or not
        // </summary>
        // <returns>True or false</returns>
        // <remarks></remarks>
	    public bool isInitial(){
		    if(initial.Equals("YES")) return true;
		    else if(initial.Equals("NO")) return false;
		    else {
			    //Debug.LogError("State.cs - Error in xml document (<State initial={YES/NO}>)"); 
			    return false;
		    }
	    }
	
        // <summary>
        // Get the list of transitions of this state
        // </summary>
        // <returns>List of transitions</returns>
        // <remarks></remarks>
	    public List<Transition> getTransitions(){
		    return this.stateTransitions;
	    }


        // <summary>
        // Get the action tag assigned to this state
        // </summary>
        // <returns>tag value</returns>
        // <remarks></remarks>
	    public int getAction(){
		    return this.action_TAG;
	    }
        // <summary>
        // Get the IN action tag assigned to this state
        // </summary>
        // <returns>IN tag value</returns>
        // <remarks></remarks>
	    public int getInAction(){
		    return this.in_action_TAG;
	    }
        // <summary>
        // Get the OUT action tag assigned to this state
        // </summary>
        // <returns>OUT tag value</returns>
        // <remarks></remarks>
	    public int getOutAction(){
		    return this.out_action_TAG;
	    }

	    #endregion	
    }
}