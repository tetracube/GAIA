using System.Collections.Generic;
using System.Runtime.CompilerServices;

// <summary>
// IA management based on Classic Finite State Machines 
// </summary>
// <remarks></remarks>
namespace GAIA{
    // <summary>
    // Classic Finite Automaton that can be Deterministic (FlagProbabilistic = NO) or probabilistic (FlagProbabilistic = YES)
    // </summary>
    // <remarks>All FAs are based on this </remarks>
    public class FA_Classic {

        //<summary> Identifier of this FA </summary>
	    protected string ID;
        //<summary> List of states of this FA </summary>
	    protected List<State> StatesList;
        //<summary> List of transitions of this FA </summary>			
	    protected List<Transition> TransitionsList;
        //<summary> Number of not added states </summary>
        protected int n_notAddedStates;
        //<summary> Number of not added transitions </summary>
        protected int n_notAddedTransitions;
        //<summary> Tag that identifies this FA </summary>
	    protected int FA_tag;
        //<summary> Position in the FA (used internally) </summary>
	    protected int positionInGraph;
        //<summary> Type of FA</summary>
	    protected FAType FAtype;
        //<summary> Flag that determines if there is one initial state in this FA</summary>						
	    protected bool existInitial;
        //<summary> Initial state</summary>			
	    protected State initial;
        //<summary> Events Routine for this FA</summary>						
	    protected string CallbackName;
        //<summary> Probabilistic</summary>
	    protected bool FlagProbabilistic;

		public enum FAType
        {
			Classic,
			Concurrent,
			Inertial,
			Stacked,
			TotalSupportedFAType
        }

		//This string list has to match exactly with the FAType, both with the order and the amount
		private static string[] TypeName =
		{
			"Classic",
			"Concurrent",
			"Inertial",
			"Stacked"
		};

        // <summary>
        // Initializes a new instance of the <see cref="T:FA_Classic">FA_Classic</see> class. 
        // </summary>
        // <param name="ID">Name of the FSM based on this FA</param>
        // <param name="tag">Tag identifier for the FSM based on this FA</param>
        // <param name="CallbackName">This events routine must be implemented</param>
        // <param name="FlagProbabilistic">If set to <see langword="true"/>, then, it is a probabilistic FA_Classic ; otherwise, it is a deterministic FA_Classic</param>
        // <remarks></remarks>
        public FA_Classic(string ID, int tag, string CallbackName, bool FlagProbabilistic){
		    this.ID = ID;
		    existInitial = false;
		    StatesList = new List<State>();
		    TransitionsList = new List<Transition>();
		    n_notAddedStates = n_notAddedTransitions = 0;
		    positionInGraph = -1; //Default value
		    FAtype = FAType.Classic;
		    FA_tag = tag;
		    this.CallbackName = CallbackName;
		    this.FlagProbabilistic = FlagProbabilistic;
	    }
	
	    // Use this for initialization
        // <summary>
        // This method allows the starting of the FSM based on this FA
        // </summary>
        // <remarks>It must be called when the FA is complete</remarks>
	    public virtual void Start () {
		    foreach(State st in StatesList){
			    if(st.initial){
				    existInitial = true;
				    initial = st;
				    positionInGraph = StatesList.IndexOf(st);
			    }
			    if(st.getSubFA()!=null){
				    st.getSubFA().Start();
			    }

		    }
	    }
	    #region ADD methods
	    /*
	     * Returns
	     *
	     */
        // <summary>
        // Add a new state node to the FSM based on this FA
        // </summary>
        // <param name="newState">New instance of State class</param>
        // <returns>
        // true if OK
	    // false if cannot be added
        //</returns>
        // <remarks></remarks>
	    public bool addState(State newState){
		    if(StatesList.Contains(newState)){
			    n_notAddedStates++;
				return false;
		    }else{
			    StatesList.Add(newState);
			    return true;
			}
		}
	
	    
        // <summary>
        // Add a new transition to the FSM based on this FA
        // </summary>
        // <param name="newTransition">New instance of Transition class</param>
        // <returns>
        // true if OK
        // false if cannot be added
        //</returns>
        // <remarks></remarks>
	    public bool addTransition(Transition newTransition){
		    if ((StatesList.Contains(newTransition.getOrigin()) && StatesList.Contains(newTransition.getFinal())) && !newTransition.getID().Equals("")){ //States that belong to newTransition are in the Graph
			    TransitionsList.Add(newTransition);//Remember to add the transition to state's transition list
			    return true;
		    }else{
			    n_notAddedTransitions++;
				return false;
			}
		}
		#endregion

		#region GET methods

		// <summary>
		//Get the tag that identifies the FSM based on this FA
		// </summary>
		// <returns>An identifier number</returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int getTag(){ return this.FA_tag; }

		// <summary>
		// Get the events routine of the FSM based on this FA
		// </summary>
		// <returns>An string with the name of the routine</returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string getCallback(){ return this.CallbackName; }

		// <summary>
		//Get current nº of states that have been added to the FSM based on this FA
		// </summary>
		// <returns>int value</returns>
		// <remarks>Only can be used if FSM_Parser is used too</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int getAddedStates(){ return StatesList.Count; }

		// <summary>
		//Get current nº of states that have not been added to the FSM based on this FA
		// </summary>
		// <returns>int value</returns>
		// <remarks>Only can be used if FSM_Parser is used too</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int getNotAddedStates(){ return n_notAddedStates; }

		// <summary>
		//Obtain current nº of transitions that have been added to the FSM based on this FA
		// </summary>
		// <returns>int value</returns>
		// <remarks>Only can be used if FSM_Parser is used too</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int getAddedTransitions(){ return TransitionsList.Count; }

		// <summary>
		//Get current nº of transitions that have not been added to the FSM based on this FA
		// </summary>
		// <returns>int value</returns>
		// <remarks>Only can be used if FSM_Parser is used too</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int getNotAddedTransitions(){ return n_notAddedTransitions; }

		// <summary>
		//Get a boolean flag that determines if exist one initial state in the FSM based on this FA
		// </summary>
		// <returns>True or False</returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool ExistInitial(){ return this.existInitial; }

		// <summary>
		// Get the StatesList
		// </summary>
		// <returns>List of states of the FSM based on this FA</returns>
		// <remarks>Current added States of the FSM based on this FA</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public List<State> getStatesList(){ return StatesList; }

		// <summary>
		// Get the TransitionsList
		// </summary>
		// <returns>List of transitions of the FSM based on this FA</returns>
		// <remarks>Current added transitions of the FSM based on this FA</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public List<Transition> getTransitionsList(){ return TransitionsList; }

		// <summary>
		// Get the state (if exist) whose identifier is ID
		// </summary>
		// <param name="ID">Identifier or name of a State </param>
		// <returns>State value or null </returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public State getStateByID(string ID){
		    foreach (State s in StatesList)
			    if(s.getID().Equals(ID)) return s;
		    return null;
	    }
		// <summary>
		// Get the state (if exist) whose identification number is tag
		// </summary>
		// <param name="tag">Identification tag</param>
		// <returns>State value or null</returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public State getStateByTag(int tag){
		    foreach(State s in StatesList)
			    if(s.getTag()==tag) return s;
		    return null;
	    }


		// <summary>
		// Get initial state in the FSM based on this FA
		// </summary>
		// <returns>Initial State value or null</returns>
		// <remarks>If the FA is Concurrent_States one, use getInitials method</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public State getInitialState(){ return this.initial; }

		// <summary>
		// Converts the FAType into its corresponding string
		// </summary>
		// <returns>The name of the FA type</returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string getFAtypeName(){ return TypeName[(int)FAtype]; }

		// <summary>
		// Overloads the FAType into its corresponding string
		// </summary>
		// <returns>The name of the FA type</returns>
		// <remarks></remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FAType getFAtype(){ return FAtype; }
		
		// <summary>
		 // Gets its FA id
		 // </summary>
		 // <returns>The name of the FSM based on this FA</returns>
		 // <remarks></remarks>
		public virtual string getFAid(){
		    return ID;
	    }
	    
        // <summary>
        // Get if the FSM based on this FA is probabilistic or not
        // </summary>
        // <returns>True or false</returns>
        // <remarks></remarks>
	    public bool isProbabilistic(){
		    return this.FlagProbabilistic;
	    }
	    #endregion
	
	    #region SET methods
	   
        // <summary>
        // This method allows to change callback events routine for the FSM based on this FA
        // </summary>
        // <param name="newCallback">Name of the new Callback routine</param>
        // <remarks>This new Callback must be implemented</remarks>
	    public void setCallback(string newCallback){
		    this.CallbackName = newCallback;
	    }
	    #endregion
	}
}
