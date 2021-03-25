using UnityEngine;
using System.Collections.Generic;

namespace GAIA{

    // <summary>
    // Stack-based Finite Automaton based on FA_Classic
    // </summary>
    // <remarks></remarks>
    public class FA_Stack : FA_Classic {

        // <summary>
        // Initializes a new instance of the <see cref="T:FSM.FA_Stack">FA_Stack</see> class. 
        // </summary>
        // <param name="ID">Name of the FSM based on this FA</param>
        // <param name="tag">Tag identifier for the FSM based on this FA</param>
        // <param name="CallbackName">This events routine must be implemented</param>
        // <param name="FlagProbabilistic">If set to <see langword="true"/>, then, it is a probabilistic FA_Stack ; otherwise, it is a deterministic FA_Stack</param>
        // <remarks></remarks>
	    public FA_Stack(string ID, int tag, string CallbackName, bool FlagProbability) : base(ID, tag, CallbackName, FlagProbability) {
		    this.ID = ID;
		    this.FAtype = "Stack-based";
		    this.FA_tag = tag;
		    this.CallbackName = CallbackName;
	    }
	
        // <summary>
        // This method allows the starting of the FSM based on this FA
        // </summary>
        // <remarks>It must be called when the FA is complete</remarks>
	    public override void Start(){
		    foreach(State st in StatesList){
			    if(st.isInitial()){
				    this.existInitial = true;
				    this.initial = st;
				    positionInGraph = StatesList.IndexOf(st);	
			    }
			    //st.setEnabledBy(EnableByEvents(st));
			    if(st.getSubFA()!=null){
				    st.getSubFA().Start();
			    }
		    }	
	    }

        // <summary>
        // Get FA type 
        // </summary>
        // <returns>The name of the FA type</returns>
        // <remarks></remarks>
	    public override string getFAtype(){
		    return this.FAtype;	
	
	    }
        // <summary>
        // Get FA id 
        // </summary>
        // <returns>The name of the FSM based on this FA</returns>
        // <remarks></remarks>
	    public override string getFAid(){
		    return this.ID;
	    }
	}
}