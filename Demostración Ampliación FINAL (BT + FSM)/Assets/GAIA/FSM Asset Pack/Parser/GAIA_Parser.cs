//Uncomment the line below to enable the use of the BT functions.
#define PANDA

using System.Collections.Generic;
using System.Xml;
using System;
using System.IO;
using UnityEditor;
using GAIA;

// Namespace that includes GAIA_Parser device to create xml data graph
namespace xmltest
{
	
    // Device that allows to extract data from xml documents that contains FSM design
	public class GAIA_Parser
	{
		//GAIA_Parser Class Attributes

		//Xml document instance to load one in memory
		XmlDocument xDoc;
		XmlDocument xDocBT;
        //Parsing Log file
        System.IO.StreamWriter file;
        // Lines to write in log file
		string LogLines;
		// String where the BT is defined for the Panda component
		string parsedBTtext;
        //Number of found errors in the FSM parsing process
		int numErrors;
		//Number of found errors in the BT parsing process
		int numErrorsBT;
		//Number of Loaded FSM
		int LoadedMachines;
        //Number of Loaded subFSM
        int LoadedSubMachines;
		//Number of Loaded BTs
		int LoadedBTs;
		//Number of Trees Loaded in a BT
		int treesAdded;
		//Auxiliar State used internally to control hierarchy
		State aux;


		// Initializes a new instance of the GAIA_Parser class.
		public GAIA_Parser()
		{
			LoadedMachines = LoadedSubMachines = LoadedBTs = numErrors = numErrorsBT = 0;
			string logPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/GAIA/FSM Asset Pack/Parser/ParsingLog.txt";
			file = new System.IO.StreamWriter(logPath);
		}

		
        // Writes a parser log in a txt file
        // logPath: The path where log file will be created
        // If logPath is an invalid value, it will be created in Assets/Parser/ folder by default
		public void WriteLog(string logPath){
			//try{
				//file = new System.IO.StreamWriter (logPath);
			//}catch(ArgumentException ar){

				//logPath = System.IO.Directory.GetCurrentDirectory()+ "/Assets/GAIA/FSM Asset Pack/Parser/ParsingLog.txt";
				//file = new System.IO.StreamWriter (logPath);
			//}

		
			LogLines +="\r\n\r\n--------SUMMARY--------";
			LogLines +="\r\nNumber of main machines: "+(LoadedMachines-LoadedSubMachines);
			LogLines +="\r\nNumber of submachines: "+LoadedSubMachines;
			LogLines +="\r\nTOTAL MACHINES ADDED: "+LoadedMachines;
			LogLines += "\r\nNUMBER OF BTs ADDED: " + LoadedBTs;

			file.WriteLine("LOG FILE\t" + System.DateTime.Now + "\r\n--------------------------------------"+LogLines);
			//Closing log file
			file.Close (); 
		}

		
        // Parses one FA from path
        // path: required parameter
        // subFSM: False by default. True is used internally
        // Returns FA value or null
        // Do not use true in subFSM param
		public FA_Classic ParsePath(string path, bool subFSM = false){

			numErrors=0;
			//numWarnings=0;
			int prob = 0;

			xDoc = new XmlDocument();
			try{
                //xDoc.Load(path);   // It is used to load XML either from a stream, TextReader, path/URL, or XmlReader
                xDoc.LoadXml(path); //  It is used to load the XML contained within a string.
			}catch(FileNotFoundException e){
				numErrors++;
				LogLines += "\r\n>>ERROR. Cannot load path: '"+path+"'.";
			}

			//To check tags
			XmlNodeList States = null, Transitions = null, statesList = null, transitionsList = null;
			bool startParseFlag = true;
			
			#region CHECKING COMMON MANDATORY TAGS
			//Extract the states and put them into a List "statesList"
			States = xDoc.GetElementsByTagName ("States");
			if (States != null) {
				if (States.Count != 0)
					statesList = ((XmlElement)States [0]).GetElementsByTagName ("State");
				else {
					numErrors++;
					LogLines += "\r\n>>ERROR. No <State> tag in FSM with path '"+path+"'.";			
					startParseFlag = false;
				}
			} else {
				numErrors++;
				LogLines += "\r\n>>ERROR. There must be a 'States' tag in FSM with path '"+path+"'.";	
				startParseFlag = false;
			}
			
			//Extract the transitions of xml document and put them into a List "transitionsList"
			Transitions = xDoc.GetElementsByTagName ("Transitions");
			if (Transitions != null) {
				if (Transitions.Count != 0)
					transitionsList = ((XmlElement)Transitions [0]).GetElementsByTagName ("Transition");
				else {
					LogLines += "\r\n>>ERROR. No <Transition> tag in FSM with path '"+path+"'.";		
					numErrors++;
					startParseFlag = false;
				}
			} else {
				LogLines += "\r\n>>ERROR. No <Transitions> tag in FSM with path '"+path+"'.";
				numErrors++;
				startParseFlag = false;
			}
			
			//Extract FSM type

			string FSMtype;
			try{
				//Extract the name of FSMtype
				if(xDoc.GetElementsByTagName ("FSMtype").Item (0).InnerText.Trim ().Length != 0){
					FSMtype = xDoc.GetElementsByTagName ("FSMtype").Item (0).InnerText;
				}else{
					LogLines += "\r\n>>ERROR. <FSMtype> tag in FSM with path '"+path+"' must be named.";
					FSMtype = "";
					numErrors++;
					startParseFlag = false;
				}
			}catch(Exception e){
				LogLines += "\r\n>>ERROR. No <FSMtype> tag in FSM with path '"+path+"'.";
				FSMtype = "";
				numErrors++;
				startParseFlag = false;
			}


			bool FlagProbabilistic = false;
			try{
				//Extract probabilistic flag
				if(xDoc.GetElementsByTagName ("FSMtype").Item (0).Attributes.Item(0).InnerText.Trim ().Length != 0){
					if(xDoc.GetElementsByTagName ("FSMtype").Item (0).Attributes.Item(0).InnerText.Equals("YES")){
						FlagProbabilistic = true;
					}
				}else{
					LogLines += "\r\n>>ERROR. <FSMtype Probabilistic> attribute in FSM with path '"+path+"' must be named {YES/NO}.";
					FSMtype = "";
					numErrors++;
					startParseFlag = false;
				}
			}catch(Exception e){
				LogLines += "\r\n>>ERROR. No <FSMtype Probabilistic> attribute in FSM with path '"+path+"'.";
				FSMtype = "";
				numErrors++;
				startParseFlag = false;
			}

			string FSMid = "";
			try{
				//Extract the name of FSMtype
				if(xDoc.GetElementsByTagName ("FSMid").Item (0).InnerText.Trim ().Length != 0){
					FSMid = xDoc.GetElementsByTagName ("FSMid").Item (0).InnerText;
				}else{
					LogLines += "\r\n>>ERROR. <FSMid> tag in FSM with path '"+path+"' must be named.";
					FSMid = "";
					numErrors++;
					startParseFlag = false;
				}
			}catch(Exception e){
				LogLines += "\r\n>>ERROR. No <FSMid> tag in FSM with path '"+path+"'.";
				FSMid = "";
				numErrors++;
				startParseFlag = false;
			}

			string CallbackName;
			try{
				//Extract the name of the procedure/routine/callback that manages current FSM events
				if(xDoc.GetElementsByTagName ("Callback").Item (0).InnerText.Trim ().Length != 0){
					CallbackName = xDoc.GetElementsByTagName ("Callback").Item (0).InnerText;
				}else{
					LogLines += "\r\n>>ERROR. <Callback> tag in FSM with path '"+path+"'.";
					CallbackName = null;
					numErrors++;
					startParseFlag = false;
				}
			}catch(Exception e){
				LogLines += "\r\n>>ERROR. No <Callback> tag in FSM with path '"+path+"'.";
				CallbackName = null;
				numErrors++;
				startParseFlag = false;
			}
			
			//Check some more if they are needed...
			#endregion
			
			#region SPECIFIC EXTRACTION (DEPENDS ON FSM type)
			if (startParseFlag) {	
				State A, B;
				int callResult = 0;
				
				//***MACHINE TYPE SWITCH***
				LogLines += "\r\n \r\n=============================================================================================";

				switch (FSMtype) {
				case "CLASSIC":
					#region CLASSIC FSM
					if(subFSM){
						LogLines += "\r\n...LOADING A (SUB)CLASSIC MACHINE named '"+FSMid+"' to current STATE";
					}else{
						LogLines += "\r\n...LOADING A CLASSIC MACHINE named '"+FSMid+"'";
					}	
					//New deterministic FSM
					FA_Classic fsm_p = new FA_Classic (FSMid, Tags.StringToTag(FSMtype), CallbackName, FlagProbabilistic);
					
					//Analize states
					for (int i=0; i< statesList.Count; i++) {
						//Add a new State
						if (statesList.Item (i).ChildNodes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).Attributes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).ChildNodes.Item (1).InnerText.Trim ().Length != 0) {
							//addState(new State(string name, string initial, int tag, int action, int in_action, int out_action))
							aux = new State(statesList.Item (i).ChildNodes.Item (0).InnerText, statesList.Item (i).Attributes.Item (0).InnerText, Tags.StringToTag(statesList.Item (i).ChildNodes.Item (0).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (1).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (2).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (3).InnerText));
							callResult = fsm_p.addState (aux);
							if (callResult == -1) {
								LogLines += "\r\n>>ERROR: State '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								#region check possible submachine
								if(statesList.Item(i).ChildNodes.Item(4).InnerText.Trim().Length!=0){
									int res = aux.addFA(ParsePath(System.IO.Directory.GetCurrentDirectory()+statesList.Item(i).ChildNodes.Item(4).InnerText, true));

									if(res == -1){
										LogLines += "\r\n>>ERROR in (SUB)FSM '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'. Cannot attach a null FSM.";
										LogLines += "\r\n \r\n>>NOT ADDED (SUB)FSM to '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
										numErrors++;
									}else{
										LoadedSubMachines++;
									}

								}			
								#endregion
							}
						} else {
							LogLines += "\r\n>>ERROR. Some state elements are null in xml specification.";
							LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
							numErrors++;
						}
					}
					callResult = 0; 
					//Analize transitions
					for (int i=0; i<transitionsList.Count; i++) {
						//...EXTRACT INFORMATION OF EACH TRANSITION
						//Extract origin and destination states from xml doc
						A = fsm_p.getStateByID (transitionsList.Item (i).ChildNodes.Item (1).InnerText);
						B = fsm_p.getStateByID (transitionsList.Item (i).ChildNodes.Item (2).InnerText);

						if(FlagProbabilistic){
							try{
								prob = Convert.ToInt32(transitionsList.Item (i).ChildNodes.Item (5).InnerText);
								if(prob<0){
									numErrors++;
									prob = 0; //default 100%
									LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". Negative value. Set to default value (0%).";
								}
							}catch(Exception e){
								numErrors++;
								prob = 100; //default 100%
								LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". NaN. Set to default value (100%).";
								
							}
						}
						//Add a new transition with ID between the states A and B (always that both are in the graph or ID != "")
						if(A!=null && B!=null){
							
							string T_ID 	= transitionsList.Item (i).ChildNodes.Item (0).InnerText;
							int T_tag	= Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (0).InnerText);
							int T_action = Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (3).InnerText);
							XmlNodeList events = transitionsList.Item(i).ChildNodes.Item(4).ChildNodes;
							
							//Transition's EventsList 
							List<FSM_Event> EventsList = new List<FSM_Event>();
							foreach(XmlNode n in events){
								EventsList.Add(new FSM_Event(n.ChildNodes.Item(0).InnerText, Tags.StringToTag(n.ChildNodes.Item(0).InnerText), n.ChildNodes.Item(1).InnerText));
							}
							Transition newT;
							if(FlagProbabilistic) newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList, prob);
							else newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList);
							//Transition newT = new Transition (transitionsList.Item (i).ChildNodes.Item (0).InnerText, A, B, Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (0).InnerText), Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (3).InnerText), Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (4).InnerText), prob);
							callResult = fsm_p.addTransition (newT);
							
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to FSM.";
							}	
							
							//Add this transition to TransitionsList of origin State
							callResult = A.addTransition (newT);
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added to that State.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to State '"+transitionsList.Item (i).ChildNodes.Item (1).InnerText+"'.";
							}
						}else{
							LogLines += "\r\n>>ERROR. States are null. Cannot add the transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.\n";
						}	
					}
					
					//START FSM
					fsm_p.Start();
					
					//CHECKING SUMMARY
					LogLines += "\r\nStates ADDED: " + fsm_p.getAddedStates () + " of " + statesList.Count + "\r\nStates NOT ADDED: " + fsm_p.getNotAddedStates () +
						"\r\nTransitions ADDED: " + fsm_p.getAddedTransitions () + " of " + transitionsList.Count + "\r\nTransitions NOT ADDED: " + fsm_p.getNotAddedTransitions () + "";

					LogLines += "\r\nNumber of errors: "+numErrors;

					if (numErrors>0){
						LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
					} else {
						if (!fsm_p.ExistInitial ()) {
							LogLines += "\r\n>>ERROR. There is not an initial state. Check XML file\r\n (Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
							numErrors++;
						} else
							LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is OK!)";
					}
			
		
					LoadedMachines++;
					return fsm_p;
					#endregion
					break;
				case "CONCURRENT_STATES":
					#region CONCURRENT-STATES FSM
					if(subFSM){
						LogLines += "\r\n...LOADING A (SUB)CONCURRENT-STATES MACHINE named '"+FSMid+"' to current STATE";
					}else{
						LogLines += "\r\n...LOADING A CONCURRENT-STATES MACHINE named '"+FSMid+"'";
					}	
					
					int num; short credits;
					try{
						num = Convert.ToInt32(xDoc.GetElementsByTagName ("Fsm").Item (0).Attributes.Item(0).InnerText);
						if(num<0){
							LogLines += "ERROR in 'MaxConcurrent number'. Negative value. Set to default value (1)";
							numErrors++;
							num = 1;
						}
					}catch(Exception e){
						LogLines += "ERROR in 'MaxConcurrent number'. Set to default value (1)";
						numErrors++;
						num = 1;
					}
					
					//New ConcurrentStates FSM with tag and num (num max of concurrentStates)
					FA_Concurrent_States fsm_cs = new FA_Concurrent_States (FSMid, Tags.StringToTag(FSMtype), num, CallbackName, FlagProbabilistic);
					
					//Analize states
					for (int i=0; i< statesList.Count; i++) {
						//Add a new State
						if (statesList.Item (i).ChildNodes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).Attributes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).ChildNodes.Item (1).InnerText.Trim ().Length != 0) {
							try{
								credits = Convert.ToInt16(statesList.Item (i).ChildNodes.Item (5).InnerText);
								if(credits<0) {
									LogLines += "ERROR in <S_Credits> of state "+statesList.Item (i).ChildNodes.Item (0).InnerText+". Negative value. Set to default value (0)";
									numErrors++;
									credits = 0;
								}
							}catch(Exception e){
								LogLines += "ERROR in <S_Credits> of state "+statesList.Item (i).ChildNodes.Item (0).InnerText+". NaN. Set to default value (0)";
								numErrors++;
								credits = 0;
							}
							aux = new State(statesList.Item (i).ChildNodes.Item (0).InnerText, statesList.Item (i).Attributes.Item (0).InnerText, Tags.StringToTag(statesList.Item (i).ChildNodes.Item (0).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (1).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (2).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (3).InnerText), credits);
							callResult = fsm_cs.addState (aux);
							if (callResult == -1) {
								LogLines += "\r\n>>ERROR. State '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								#region check possible submachine
								if(statesList.Item(i).ChildNodes.Item(4).InnerText.Trim().Length!=0){

									int res = aux.addFA(ParsePath(System.IO.Directory.GetCurrentDirectory()+statesList.Item(i).ChildNodes.Item(4).InnerText, true));

									if(res == -1){
										LogLines += "\r\n>>ERROR in (SUB)FSM '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'. Cannot attach a null FSM.";
										LogLines += "\r\n \r\n>>NOT ADDED (SUB)FSM to '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
										numErrors++;
									}else{
										LoadedSubMachines++;
									}
								}			
								#endregion
							}
						} else {
							LogLines += "\r\n>>ERROR. Some state elements are null in xml specification.";
							LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
							numErrors++;
						}
					}
					callResult = 0;
					//Analize transitions
					for (int i=0; i<transitionsList.Count; i++) {
						//...EXTRACT INFORMATION OF EACH TRANSITION
						
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(0).InnerText); //T-Name
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(1).InnerText); //T-Origin
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(2).InnerText); //T-Destination
						
						//Extract origin and destination states from xml doc
						A = fsm_cs.getStateByID (transitionsList.Item (i).ChildNodes.Item (1).InnerText);
						B = fsm_cs.getStateByID (transitionsList.Item (i).ChildNodes.Item (2).InnerText);
						if(FlagProbabilistic){
							try{
								prob = Convert.ToInt32(transitionsList.Item (i).ChildNodes.Item (5).InnerText);
								if(prob<0){
									numErrors++;
									prob = 0; //default 100%
									LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". Negative value. Set to default value (0%).";
								}
							}catch(Exception e){
								numErrors++;
								prob = 100; //default 100%
								LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". NaN. Set to default value (100%).";
								
							}
						}
						//Add a new transition with ID between the states A and B (always that both are in the graph or ID != "")
						if(A!=null && B!=null){

							string T_ID 	= transitionsList.Item (i).ChildNodes.Item (0).InnerText;
							int T_tag	= Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (0).InnerText);
							int T_action = Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (3).InnerText);
							XmlNodeList events = transitionsList.Item(i).ChildNodes.Item(4).ChildNodes;
							
							//Transition's EventsList 
							List<FSM_Event> EventsList = new List<FSM_Event>();
							foreach(XmlNode n in events){
								EventsList.Add(new FSM_Event(n.ChildNodes.Item(0).InnerText, Tags.StringToTag(n.ChildNodes.Item(0).InnerText), n.ChildNodes.Item(1).InnerText));
							}
							
							Transition newT;
							if(FlagProbabilistic) newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList, prob);
							else newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList);
							callResult = fsm_cs.addTransition (newT);
							
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to FSM.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to FSM.";
							}	
							
							//Add this transition to TransitionsList of origin State
							callResult = A.addTransition (newT);
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added to that State.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to State '"+transitionsList.Item (i).ChildNodes.Item (1).InnerText+"'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to State '"+transitionsList.Item (i).ChildNodes.Item (1).InnerText+"'.";
							}
						}else{
							LogLines += "\r\n>>ERROR. Transition states are null. Cannot add the transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.\n";
							numErrors++;
						}	
					}
					
					//START FSM
					fsm_cs.Start();
					
					//CHECKING SUMMARY
					LogLines += "\r\nStates ADDED: " + fsm_cs.getAddedStates () + " of " + statesList.Count + "\r\nStates NOT ADDED: " + fsm_cs.getNotAddedStates () +
						"\r\nTransitions ADDED: " + fsm_cs.getAddedTransitions () + " of " + transitionsList.Count + "\r\nTransitions NOT ADDED: " + fsm_cs.getNotAddedTransitions () + "";

					LogLines += "\r\nNumber of errors: "+numErrors;

					if (numErrors>0){
						LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
					} else {
						if (!fsm_cs.ExistInitial ()) {
							LogLines += "\r\n>>ERROR. There is not an initial state. Check XML file\r\n (Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
							numErrors++;
						} else if(fsm_cs.getInitials().Count > fsm_cs.getMaxConcurrent()){
							LogLines += "\r\n>>ERROR. Num of Initials cannot be greater than max of concurrents. Check XML file\r\n (Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
							numErrors++;
						}else
							LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is OK!)";
					}
					LogLines +="\r\n";

					LoadedMachines++;

					return fsm_cs;
					#endregion
					break;

				case "INERTIAL":		//(...specific Inertial FSM code here...)
					#region INERTIAL FSM
					if(subFSM){
						LogLines += "\r\n...LOADING A (SUB)INERTIAL MACHINE named '"+FSMid+"' to current STATE";
					}else{
						LogLines += "\r\n...LOADING A INERTIAL MACHINE named '"+FSMid+"'";
					}	
					//New Inertial FSM
					FA_Inertial fsm_i = new FA_Inertial (FSMid, Tags.StringToTag(FSMtype), CallbackName, FlagProbabilistic);
					
					int lat;
					//Analize states
					for (int i=0; i< statesList.Count; i++) {
						//check latency
						try{
							lat = Convert.ToInt32(statesList.Item (i).ChildNodes.Item (5).InnerText);
							if(lat<0){
								LogLines += "\r\n>>ERROR. Latency of State '"+statesList.Item (i).ChildNodes.Item (0).InnerText+"' cannot be negative. Set to default value (0)";
								lat = 0;
								numErrors++;
							}
						}catch(Exception e){
							LogLines += "\r\n>>ERROR. Latency of State '"+statesList.Item (i).ChildNodes.Item (0).InnerText+"'. NaN. Set to default value (0)";
							lat = 0;
							numErrors++;
						}
						//Add a new State with latency
						if (statesList.Item (i).ChildNodes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).Attributes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).ChildNodes.Item (1).InnerText.Trim ().Length != 0) {
							aux = new State (statesList.Item (i).ChildNodes.Item (0).InnerText, statesList.Item (i).Attributes.Item (0).InnerText, Tags.StringToTag(statesList.Item (i).ChildNodes.Item (0).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (1).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (2).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (3).InnerText), lat);
							callResult = fsm_i.addState (aux);
							if (callResult == -1) {
								LogLines += "\r\n>>ERROR. State '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								#region check possible submachine
								if(statesList.Item(i).ChildNodes.Item(4).InnerText.Trim().Length!=0){

									int res = aux.addFA(ParsePath(System.IO.Directory.GetCurrentDirectory()+statesList.Item(i).ChildNodes.Item(4).InnerText, true));

									if(res == -1){
										LogLines += "\r\n>>ERROR in (SUB)FSM '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'. Cannot attach a null FSM.";
										LogLines += "\r\n \r\n>>NOT ADDED (SUB)FSM to '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
										numErrors++;
									}else{
										LoadedSubMachines++;
									}

								}			
								#endregion
							}
						} else {
							LogLines += "\r\n>>ERROR. Some state elements are null in xml specification. State num: " + i;
							LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
							numErrors++;
						}
					}
					callResult = 0;
					//Analize transitions
					for (int i=0; i<transitionsList.Count; i++) {
						//...EXTRACT INFORMATION OF EACH TRANSITION
						
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(0).InnerText); //T-Name
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(1).InnerText); //T-Origin
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(2).InnerText); //T-Destination
						
						//Extract origin and destination states from xml doc
						A = fsm_i.getStateByID (transitionsList.Item (i).ChildNodes.Item (1).InnerText);
						B = fsm_i.getStateByID (transitionsList.Item (i).ChildNodes.Item (2).InnerText);

						if(FlagProbabilistic){
							try{
								prob = Convert.ToInt32(transitionsList.Item (i).ChildNodes.Item (5).InnerText);
								if(prob<0){
									numErrors++;
									prob = 0; //default 100%
									LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". Negative value. Set to default value (0%).";
								}
							}catch(Exception e){
								numErrors++;
								prob = 100; //default 100%
								LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". NaN. Set to default value (100%).";
								
							}
						}
						//Add a new transition with ID between the states A and B (always that both are in the graph or ID != "")
						if(A!=null && B!=null){
							
							string T_ID 	= transitionsList.Item (i).ChildNodes.Item (0).InnerText;
							int T_tag	= Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (0).InnerText);
							int T_action = Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (3).InnerText);
							XmlNodeList events = transitionsList.Item(i).ChildNodes.Item(4).ChildNodes;
							
							//Transition's EventsList 
							List<FSM_Event> EventsList = new List<FSM_Event>();
							foreach(XmlNode n in events){
								EventsList.Add(new FSM_Event(n.ChildNodes.Item(0).InnerText, Tags.StringToTag(n.ChildNodes.Item(0).InnerText), n.ChildNodes.Item(1).InnerText));
							}
							
							Transition newT;
							if(FlagProbabilistic) newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList, prob);
							else newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList);
							callResult = fsm_i.addTransition (newT);
							
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to FSM.";
							}	
							
							//Add this transition to TransitionsList of origin State
							callResult = A.addTransition (newT);
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added to that State.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to State '"+transitionsList.Item (i).ChildNodes.Item (1).InnerText+"'.";
							}
						}else{
							LogLines += "\r\n>>ERROR. States are null. Cannot add the transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.\n";
						}	
					}
					
					//START FSM
					fsm_i.Start();
					
					//CHECKING SUMMARY
					LogLines += "\r\nStates ADDED: " + fsm_i.getAddedStates () + " of " + statesList.Count + "\r\nStates NOT ADDED: " + fsm_i.getNotAddedStates () +
						"\r\nTransitions ADDED: " + fsm_i.getAddedTransitions () + " of " + transitionsList.Count + "\r\nTransitions NOT ADDED: " + fsm_i.getNotAddedTransitions () + "";

					LogLines += "\r\nNumber of errors: "+numErrors;

					if (numErrors>0){
						LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
					} else {
						if (!fsm_i.ExistInitial ()) {
							LogLines += "\r\n>>ERROR: There is not an initial state. Check XML file\r\n (Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
							numErrors++;
						} else
							LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is OK!)";
					}

					LoadedMachines++;

					return fsm_i;
					#endregion
					break;//End of Inertial
				
				case "STACK_BASED":		//(...specific Stack-based FSM code here...)
					#region STACK-BASED FSM
					if(subFSM){
						LogLines += "\r\n...LOADING A (SUB)STACK-BASED MACHINE named '"+FSMid+"' to current STATE";
					}else{
						LogLines += "\r\n...LOADING A STACK-BASED MACHINE named '"+FSMid+"'";
					}	
					//New stack-based FSM
					FA_Stack fsm_s = new FA_Stack (FSMid, Tags.StringToTag(FSMtype), CallbackName, FlagProbabilistic);
					
					uint pri;
					//Analize states
					for (int i=0; i< statesList.Count; i++) {
						try{
							pri = Convert.ToUInt32(statesList.Item (i).ChildNodes.Item (5).InnerText);
						}catch(Exception e){
							LogLines += "\r\n>>ERROR. State '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "' priority is NaN or negative. Set to default value (0). \n";
							numErrors++;
							pri = 0;
						}
						//Add a new State
						if (statesList.Item (i).ChildNodes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).Attributes.Item (0).InnerText.Trim ().Length != 0 && statesList.Item (i).ChildNodes.Item (1).InnerText.Trim ().Length != 0) {
							aux = new State (statesList.Item (i).ChildNodes.Item (0).InnerText, statesList.Item (i).Attributes.Item (0).InnerText, Tags.StringToTag(statesList.Item (i).ChildNodes.Item (0).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (1).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (2).InnerText), Tags.StringToTag(statesList.Item (i).ChildNodes.Item (3).InnerText), pri);
							callResult = fsm_s.addState (aux);
							if (callResult == -1) {
								LogLines += "\r\n>>ERROR: State '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								#region check possible submachine
								if(statesList.Item(i).ChildNodes.Item(4).InnerText.Trim().Length!=0){

									int res = aux.addFA(ParsePath(System.IO.Directory.GetCurrentDirectory()+statesList.Item(i).ChildNodes.Item(4).InnerText, true));

									if(res == -1){
										LogLines += "\r\n>>ERROR in (SUB)FSM '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'. Cannot attach a null FSM.";
										LogLines += "\r\n \r\n>>NOT ADDED (SUB)FSM to '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
										numErrors++;
									}else{
										LoadedSubMachines++;
									}

								}			
								#endregion
								
							}
						} else {
							LogLines += "\r\n>>ERROR: Some state elements are null in xml specification.";
							LogLines += "\r\n \r\n>>NOT ADDED STATE '" + statesList.Item (i).ChildNodes.Item (0).InnerText + "'.";
							numErrors++;
						}
					}
					callResult = 0;
					//Analize transitions
					for (int i=0; i<transitionsList.Count; i++) {
						//...EXTRACT INFORMATION OF EACH TRANSITION
						
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(0).InnerText); //T-Name
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(1).InnerText); //T-Origin
						//Debug.Log(transitionsList.Item(i).ChildNodes.Item(2).InnerText); //T-Destination
						
						//Extract origin and destination states from xml doc
						A = fsm_s.getStateByID (transitionsList.Item (i).ChildNodes.Item (1).InnerText);
						B = fsm_s.getStateByID (transitionsList.Item (i).ChildNodes.Item (2).InnerText);

						if(FlagProbabilistic){
							try{
								prob = Convert.ToInt32(transitionsList.Item (i).ChildNodes.Item (5).InnerText);
								if(prob<0){
									numErrors++;
									prob = 0; //default 100%
									LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". Negative value. Set to default value (0%).";
								}
							}catch(Exception e){
								numErrors++;
								prob = 100; //default 100%
								//UnityEngine.Debug.LogWarning("Exception to Transition with origin: "+A.getID()+" and destination: "+B.getID());
								LogLines += "\r\n>>ERROR. Transition probability with origin: "+A.getID()+" and destination: "+B.getID()+". NaN. Set to default value (100%).";
								
							}
						}
						//Add a new transition with ID between the states A and B (always that both are in the graph or ID != "")
						if(A!=null && B!=null){
							string T_ID 	= transitionsList.Item (i).ChildNodes.Item (0).InnerText;
							int T_tag	= Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (0).InnerText);
							int T_action = Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (3).InnerText);
							XmlNodeList events = transitionsList.Item(i).ChildNodes.Item(4).ChildNodes;
							
							//Transition's EventsList 
							List<FSM_Event> EventsList = new List<FSM_Event>();
							foreach(XmlNode n in events){
								EventsList.Add(new FSM_Event(n.ChildNodes.Item(0).InnerText, Tags.StringToTag(n.ChildNodes.Item(0).InnerText), n.ChildNodes.Item(1).InnerText));
							}
							
							Transition newT;
							if(FlagProbabilistic) newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList, prob);
							else newT = new Transition (T_ID, A, B, T_tag, T_action, EventsList);							//Transition newT = new Transition (transitionsList.Item (i).ChildNodes.Item (0).InnerText, A, B, Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (0).InnerText), Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (3).InnerText), Tags.StringToTag(transitionsList.Item (i).ChildNodes.Item (4).InnerText));
							callResult = fsm_s.addTransition (newT);
							
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to FSM.";
							}	
							
							//Add this transition to TransitionsList of origin State
							callResult = A.addTransition (newT);
							if(callResult == -1){
								LogLines += "\r\n>>ERROR. Transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' has already been added to that State.\n";
								LogLines += "\r\n \r\n>>NOT ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.";
								numErrors++;
							} else {
								LogLines += "\r\n>>ADDED TRANSITION '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "' to State '"+transitionsList.Item (i).ChildNodes.Item (1).InnerText+"'.";
							}
						}else{
							LogLines += "\r\n>>ERROR. States are null. Cannot add the transition '" + transitionsList.Item (i).ChildNodes.Item (0).InnerText + "'.\n";
						}	
					}
					
					//START FSM
					fsm_s.Start();
					
					//CHECKING SUMMARY
					LogLines += "\r\nStates ADDED: " + fsm_s.getAddedStates () + " of " + statesList.Count + "\r\nStates NOT ADDED: " + fsm_s.getNotAddedStates () +
						"\r\nTransitions ADDED: " + fsm_s.getAddedTransitions () + " of " + transitionsList.Count + "\r\nTransitions NOT ADDED: " + fsm_s.getNotAddedTransitions () + "";

					LogLines += "\r\nNumber of errors: "+numErrors;

					if (numErrors>0){
						LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
					} else {
						if (!fsm_s.ExistInitial ()) {
							LogLines += "\r\n>>ERROR. There is not an initial state. Check XML file\r\n (Parsing of " + FSMtype + " Machine named '"+FSMid+"' is INCONSISTENT)";
							numErrors++;
						} else
							LogLines += "\r\n \r\n(Parsing of " + FSMtype + " Machine named '"+FSMid+"' is OK!)";
					}

					LoadedMachines++;

					return fsm_s;
					#endregion
					break;
					
				default:
					LogLines += "\r\n>>ERROR. UNKNOWN FSM Type for Machine named '"+FSMid+"'";
					numErrors++;
					break;	
				}
			} else {
				LogLines += "\r\n >>ERROR. Cannot PARSE xml file. Mandatory tags are missing for FSM with path '"+path+"'.";
				numErrors++;
			}
			#endregion
			LogLines += "\r\n \r\nCritical errors: " + numErrors;

			file.WriteLine (LogLines);
			LogLines="";
			return null;
		}

#if (PANDA)

		// Function used to parse the childs of a tree
		// CN: xml node containing the child nodes to add
		// tabCount: int representing the ammount of tabulations to add, depending on the level of depth of the child in the tree
		public void addChildNodes(XmlNode CN, int tabCount)
        {
			string tab = "\t";
			string tabCollector = null;

			for (int i = 0; i < CN.ChildNodes.Count; i++)
			{
				for (int j = 0; j < tabCount; j++)
                {
					tabCollector += tab;
                }
				if(CN.ChildNodes.Item(i).ChildNodes.Item(0).InnerText == "tree")
                {
                    try 
					{ 
						parsedBTtext += tabCollector + "tree(" + '\u0022' + CN.ChildNodes.Item(i).ChildNodes.Item(1).InnerText + '\u0022' + ")\r\n";
                    }
                    catch (Exception e)
                    {
						LogLines += "\r\n>>ERROR. CHILD NODE " + '\u0022' + CN.ChildNodes.Item(i).ChildNodes.Item(1).InnerText + '\u0022' + " COULD NOT BE ADDED.";
						numErrorsBT++;
					}
					
				} else
                {
					if(CN.ChildNodes.Item(i).ChildNodes.Item(0).InnerText.Trim().Length != 0) {
						try
						{
							if (CN.ChildNodes.Item(i).ChildNodes.Item(1).InnerText.Trim().Length != 0) { 
								parsedBTtext += tabCollector + CN.ChildNodes.Item(i).ChildNodes.Item(1).InnerText + "\r\n";
                            }
                            else
                            {
								LogLines += "\r\n>>ERROR. CHILD NODE FROM THE TREE N� " + treesAdded + " HAS ITS CONTENT EMPTY. PLEASE DEFINE THE CONTENTS OF THE NODE";
								numErrorsBT++;
							}
						}
						catch (Exception e)
						{
							LogLines += "\r\n>>ERROR. CHILD NODE " + '\u0022' + CN.ChildNodes.Item(i).ChildNodes.Item(1).InnerText + '\u0022' + " COULD NOT BE ADDED.";
							numErrorsBT++;
						}
                    }
                    else
                    {
						LogLines += "\r\n>>ERROR. CHILD NODE FROM THE TREE N� " + treesAdded + " HAS NO SPECIFIED TYPE. PLEASE SPECIFY EITHER A TREE OR A NODE";
						numErrorsBT++;
					}
				}
				if(CN.ChildNodes.Item(i).ChildNodes.Count > 2)
                {
                    try
                    {
                        if (CN.ChildNodes.Item(i).ChildNodes.Item(2).ChildNodes.Count > 0 && CN.ChildNodes.Item(i).ChildNodes.Item(2).Name == "Child_Nodes") {
							addChildNodes(CN.ChildNodes.Item(i).ChildNodes.Item(2), tabCount + 1);
                        }
                        else
                        {
							LogLines += "\r\n>>ERROR. COULD NOT ADD SUBCHILDS OF THE TREE N� " + treesAdded + ". PLEASE VERIFY THE DEFINITION OF THIS TREE IS CORRECT";
							numErrorsBT++;
						}
						
					}
					catch (Exception e)
                    {
						LogLines += "\r\n>>ERROR. COULD NOT ADD NODE " + '\u0022' + CN.ChildNodes.Item(i).ChildNodes.Item(1).InnerText + '\u0022' + " CHILD NODES";
						numErrorsBT++;
					}
                }
				tabCollector = null;
			}
			parsedBTtext += "\r\n";
		}

		// Parses a BT from the contents of an xml file
		// content: string of the contents of the xml file
		// Returns a string array containing the BT id and definition in the form of a string
		public string[] ParseBT(string content)
        {
			string[] parsedbt = new string[2];
			parsedBTtext = null;

			numErrorsBT = 0;

			xDocBT = new XmlDocument();
			try
			{
				//xDocBT.Load(content);   // It is used to load XML either from a stream, TextReader, path/URL, or XmlReader
				xDocBT.LoadXml(content); //  It is used to load the XML contained within a string.
			}
			catch (FileNotFoundException e)
			{
				numErrorsBT++;
				LogLines += "\r\n>>ERROR. Cannot load file: '" + content + "'.";
			}

			XmlNodeList Trees = null, treesList = null;
			bool startParseFlagBT = true;

			//Extract the trees and put them into a List "Trees"
			Trees = xDocBT.GetElementsByTagName("Trees");
			if (Trees != null)
			{
				if (Trees.Count != 0)
					treesList = ((XmlElement)Trees[0]).GetElementsByTagName("Tree");
				else
				{
					numErrors++;
					LogLines += "\r\n>>ERROR. No <Tree> tag in BT with contents '" + content + "'.";
					startParseFlagBT = false;
				}
			}
			else
			{
				numErrors++;
				LogLines += "\r\n>>ERROR. There must be a 'Trees' tag in BT with contents '" + content + "'.";
				startParseFlagBT = false;
			}

			string BTid = "";
			try
			{
				//Extract the id of the BT
				if (xDocBT.GetElementsByTagName("BTid").Item(0).InnerText.Trim().Length != 0)
				{
					BTid = xDocBT.GetElementsByTagName("BTid").Item(0).InnerText;
				}
				else
				{
					LogLines += "\r\n>>ERROR. <BTid> tag in BT with contents '" + content + "' must be named.";
					BTid = "";
					numErrors++;
					startParseFlagBT = false;
				}
			}
			catch (Exception e)
			{
				LogLines += "\r\n>>ERROR. No <BTid> tag in BT with contents '" + content + "'.";
				BTid = "";
				numErrors++;
				startParseFlagBT = false;
			}

			if (startParseFlagBT)
            {
				LogLines += "\r\n \r\n=============================================================================================";
				LogLines += "\r\n...LOADING A BT named '" + BTid + "'";

				//Analize trees
				for (int i = 0; i < treesList.Count; i++) {
					if(treesList.Item(i).Attributes.Item(0).Value == "YES")
                    {
						parsedBTtext += "tree(" + '\u0022' + "Root" + '\u0022' + ")\r\n";
						treesAdded++;
					} else
                    {
                        try { 
							if(treesList.Item(i).FirstChild.InnerText.Trim().Length != 0) {
								parsedBTtext += "tree(" + '\u0022' + treesList.Item(i).FirstChild.InnerText + '\u0022' + ")\r\n";
								treesAdded++;
							}
                            else {
								LogLines += "\r\n>>ERROR. COULD NOT ADD TREE " + treesAdded + ". NAME FIELD IS EMPTY";
								numErrorsBT++;
							}
							
						}
						catch (Exception e)
                        {
							LogLines += "\r\n>>ERROR. COULD NOT ADD TREE " + '\u0022' + treesList.Item(i).FirstChild.InnerText + '\u0022' + ". THERE IS A PROBLEM WITH ITS NAME";
							numErrorsBT++;
						}
					}
					if(treesList.Item(i).ChildNodes.Count > 1)
                    {
                        try { 
							addChildNodes(treesList.Item(i).ChildNodes.Item(1), 1);
						}
                        catch (Exception e)
						{
							LogLines += "\r\n>>ERROR. COULD NOT ADD TREE " + '\u0022' + treesList.Item(i).FirstChild.InnerText + '\u0022' + " CHILD NODES";
							numErrorsBT++;
						}
                    }
					LogLines += "\r\n>>ADDED TREE " + treesList.Item(i).FirstChild.InnerText + " AND ITS CHILDS.";
				}
				LogLines += "\r\nTrees Added: " + treesAdded + " out of " + treesList.Count;

				if(numErrorsBT == 0)
				{
					LogLines += "\r\n\r\n(Parsing of BT named " + '\u0022' + BTid + '\u0022' + " is OK!)";
                }
                else
                {
					LogLines += "\r\n\r\n(Parsing of BT named " + '\u0022' + BTid + '\u0022' + " is NOT OK!)";
				}
				LoadedBTs++;
			}
			else
			{
				LogLines += "\r\n >>ERROR. Cannot PARSE xml file.";
				numErrorsBT++;
			}

			LogLines += "\r\n \r\nCritical errors parsing BTs: " + numErrorsBT + "\r\n";

			file.WriteLine (LogLines);
			LogLines = "";
			treesAdded = 0;

			parsedbt[0] = BTid;
			parsedbt[1] = parsedBTtext;

			return parsedbt;
		}
#endif
	}
}