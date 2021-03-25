using System.Collections.Generic;

namespace GAIA{

    // <summary>
    // Transition object class
    // </summary>
    // <remarks></remarks>
public class Transition {

	//Transition Class attributes

    // <summary>Name that identifies this transition</summary>
    string transitionName;
    // <summary>Transition tag identifier</summary>
	int transition_TAG;
    // <summary>Action tag identifier of this transition</summary>
    int action_TAG;
    // <summary>State origin from which this transition leaves</summary>
	State origin;
    // <summary>State final to which this transition arrives</summary>
    State final;
    // <summary>Collection of events that enable this transition</summary>
	List<FSM_Event> EventsList;
    //<summary>Probability of execution of this transition. Only used if the FA is probabilistic </summary>
	double? probability; 
	
	
    // <summary>
    // Initializes a new instance of the <see cref="T:FSM.Transition">Transition</see> class. 
    // </summary>
    // <param name="ID">Transition's identifier name</param>
    // <param name="A">State origin</param>
    // <param name="B">State destination</param>
    // <param name="transition_tag">Transition's tag identifier</param>
    // <param name="action_tag">Transition's action tag identifier</param>
    // <param name="EventsList">List of events that enable this transition</param>
    // <remarks>Used in all FAs</remarks>
	public Transition(string ID, State A, State B, int transition_tag, int action_tag, List<FSM_Event> EventsList){
		this.transitionName = ID;
		this.origin = A;
		this.final = B;
		this.transition_TAG = transition_tag;
		this.action_TAG = action_tag;
		this.EventsList = EventsList;
	}
    // <summary>
    // Initializes a new instance of the <see cref="T:FSM.Transition">Transition</see> class. 
    // </summary>
    // <param name="ID">Transition's identifier name</param>
    // <param name="A">State origin</param>
    // <param name="B">State destination</param>
    // <param name="transition_tag">Transition's tag identifier</param>
    // <param name="action_tag">Transition's action tag identifier</param>
    // <param name="EventsList">List of events that enable this transition</param>
    // <param name="probability">Probability between 0 and 100. Only used if the FA is probabilistic</param>
    // <remarks>It only can be used if FA is probabilistic. Its use does not make sense in the other FA</remarks>
	public Transition(string ID, State A, State B, int transition_tag, int action_tag,  List<FSM_Event> EventsList, int probability){
		this.transitionName = ID;
		this.origin = A;
		this.final = B;
		this.transition_TAG = transition_tag;
		this.action_TAG = action_tag;
		this.EventsList = EventsList;
		
		//Probability cannot be superior to 100 or inferior to 0
		if(probability>100) this.probability = 100;
		else if(probability<0) this.probability = 0;
		else this.probability = probability;
	}
	
	#region GET methods
	
    // <summary>
    // Get transition's tag identifier
    // </summary>
    // <returns>transition's tag value</returns>
    // <remarks></remarks>
	public int getTag(){
		return this.transition_TAG;
	}

    // <summary>
    // Get transition's action tag identifier
    // </summary>
    // <returns>transition's action tag value</returns>
    // <remarks></remarks>
	public int getAction(){
		return this.action_TAG;
	}
	//returns events list
    // <summary>
    // Get the specified list of events that can enable this transition
    // </summary>
    // <returns>List FSM_Events list attached to this transition </returns>
    // <remarks>It can be empty</remarks>
	public List<FSM_Event> getEvents(){
		return this.EventsList;
	}

	
    // <summary>
    // Get probability attached to this transition. 100% default value
    // </summary>
    // <returns>Double number</returns>
    // <remarks></remarks>
	public double? getProbability(){
		if(this.probability!=null) return this.probability;
		else return 100;
	}
	
    // <summary>
    // Get the origin state of this transition
    // </summary>
    // <returns>State object</returns>
    // <remarks></remarks>
	public State getOrigin(){
		return this.origin;
	}
    // <summary>
    // Get the destination state of this transition
    // </summary>
    // <returns>State object</returns>
    // <remarks></remarks>
	public State getFinal(){
		return this.final;
	}
	
    // <summary>
    // Get transition's name identifier
    // </summary>
    // <returns>A string value with the ID or null value</returns>
    // <remarks></remarks>
	public string getID(){
		if(this.transitionName!=null) return this.transitionName;
		else return null;
	}
	#endregion
	
	#region SET methods

    // <summary>
    // Set this transition's probability value
    // </summary>
    // <returns>
    // 1 if OK
    //-1 if error. This transition is not probabilistic
    //</returns>
    // <remarks></remarks>
	public int setProbability(double newProbability){
		if(this.probability!=null){
			this.probability = newProbability;
			return 1;
		}else
			return -1;
	}
	
	#endregion
}
}
