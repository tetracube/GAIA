//using UnityEngine;
using System.Collections;

namespace GAIA{

    // <summary>
    // FSM_Event class used to create events in the loading phase
    // </summary>
    // <remarks></remarks>
	public class FSM_Event {

        //<summary>BASIC type int</summary>
		private const int BASIC 	= 0;
        //<summary>STACKABLE type int. Used in FA_Stack</summary>
		private const int STACKABLE = 1;
        //<summary>Identifier or name of this FSM_Event</summary>
        private string Event_id;
        //<summary>Identifier tag number of this FSM_Event</summary>
        private int Event_tag;

        //<summary>Type of FSM_Event (BASIC or STACKABLE)</summary>
        private int iEvent_type;

        // <summary>
        // Initializes a new instance of the <see cref="T:FSM.FSM_Event">FSM_Event</see> class. 
        // </summary>
        // <param name="id">Name of this FSM_Event</param>
        // <param name="event_tag">Identifier tag of this FSM_Event</param>
        // <param name="type">"BASIC" or "STACKABLE"</param>
        // <remarks></remarks>
		public FSM_Event (string id, int event_tag, string type) {
			this.Event_id = id;
			this.Event_tag = event_tag;
			
			//Type of event
			if(type.Equals("STACKABLE")) iEvent_type = STACKABLE;
			else if(type.Equals("BASIC")) iEvent_type = BASIC;
			//else if(type.Equals("HIERARCHICAL")) iEvent_type = HIERARCHICAL;
			else {
				iEvent_type = BASIC;
			}
		}

        // <summary>
        // Get this FSM_Event identifier name
        // </summary>
        // <returns> string value </returns>
        // <remarks></remarks>
		public string getID(){
			return this.Event_id;
		}
        // <summary>
        // Get this FSM_Event identifier tag
        // </summary>
        // <returns> int value </returns>
        // <remarks></remarks>
		public int getEventTag(){
			return this.Event_tag;
		}
        // <summary>
        // Get this FSM_Event type (BASIC or STACKABLE)
        // </summary>
        // <returns> int value </returns>
        // <remarks></remarks>
		public int getEventType(){
			return iEvent_type;
		}
	}
}
