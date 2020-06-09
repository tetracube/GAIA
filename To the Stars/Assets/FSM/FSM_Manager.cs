using System.Collections.Generic;
using System;
using UnityEngine; // ngs

namespace FSM{
// <summary>
// Manager of FSMs
// </summary>
// <remarks></remarks>
    public class FSM_Manager{


    //FSM_Manager Class Attributes
    // <summary>
    // An instance of an FSM_Parser (filled with FSM_Manager(parser) constructor.
    // </summary>
    xmltest.FSM_Parser parser;
    // <summary>
    //Dictionary to add a finite automata with FAtype + FAid (Tuple) and the FA itself
    // </summary>
    Dictionary<Tuple, FA_Classic> FSM_dic;


    // <summary>
    // Initializes a new instance of the <see cref="T:FSM_Manager">FSM_Manager</see> class with an instance of FSM_Parser. 
    // </summary>
    // <param name="parser">Instance of a FSM_Parser</param>
    // <remarks></remarks>
    public FSM_Manager(xmltest.FSM_Parser parser)
    {
        this.parser = parser;
        this.FSM_dic = new Dictionary<Tuple, FA_Classic>();
    }

    // <summary>
    // Initializes a new instance of the <see cref="T:FSM_Manager">FSM_Manager</see> class. 
    // </summary>
    // <remarks></remarks>
    public FSM_Manager()
    {
        this.FSM_dic = new Dictionary<Tuple, FA_Classic>();
    }

    
    // <summary>
    //Struct that allows to add a Finite automata with type+ID
    // </summary>
    // <remarks></remarks>
    public struct Tuple
    {
        int FSMtype;
        string FSMid;
        public Tuple(int FSMtype, string FSMid)
        {
            this.FSMtype = FSMtype;
            this.FSMid = FSMid;
        }
    }

    // <summary>
    // Add a machine (passed as FA parameter) to this FSM_Manager 
    // </summary>
    // <param name="fsm"></param>
    // <returns>
    // 1 if OK
    //-1 if cannot be added
    //</returns>
    // <remarks></remarks>
    public int addFSM(FA_Classic fsm)
    {
        try
        {
            //Add the FA to the attribute FSM_dic
            FSM_dic.Add(new Tuple(fsm.getTag(), fsm.getFAid()), fsm);
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }


    // <summary>
    // Adds a machine (located in path) to this FSM_Manager
    // </summary>
    // <param name="path"></param>
    // <returns>
    // 1 if OK
    //-1 if an FSM_Parser is required to use this method
    //-2 if cannot be added (wrong parameters or repeated FA)
    //-3 if there are errors parsing file located in path
    //</returns>
    // <remarks></remarks>
    public int addFSM(string path)
    {
        if (this.parser == null)
        {
            return -1;
        }
        else
        {
            //Invoke parser with path
            FA_Classic parsedfsm = parser.ParsePath(path);
            if (parsedfsm != null)
            {
                try
                {
                    FSM_dic.Add(new Tuple(parsedfsm.getTag(), parsedfsm.getFAid()), parsedfsm);
                    return 1;
                }
                catch (Exception e)
                {
                    return -2;
                }
            }
            else return -3;
        }
    }

    /* 
    Returns
	 
    // <summary>
    // Removes a machine that had been added to this FSM_Manager
    // </summary>
    // <param name="fsm"></param>
    // <returns>
    // 1 if OK
    //-1 if cannot remove that machine (corrupted fsm object or it does not exist)*/
    // </returns>
    // <remarks></remarks>
    public int deleteFSM(FA_Classic fsm)
    {
        try
        {
            FSM_dic.Remove(new Tuple(fsm.getTag(), fsm.getFAid()));
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }


    // <summary>
    // Creates a returnable FSM_Machine to an entity (character)
    // </summary>
    // <param name="character"></param>
    // <param name="FSM_type"></param>
    // <param name="FSM_id"></param>
    // <returns>
    // The correct value of FSM_Machine or NULL (wrong parameters or that FSM does not exist))
    //</returns>
    // <remarks>It tries to find a matching between the passed parameters and an instance in the internal FSM Dictionary (FSM_Dic)</remarks>
    public FSM_Machine createMachine(System.Object character, int FSM_type, string FSM_id)
    {
        try
        {
            return new FSM_Machine(FSM_dic[new Tuple(FSM_type, FSM_id)], character);
        }
        catch (Exception e)
        {
            Debug.Log("EXCEPTION: " + e); // ngs
            return null;
        }
    }
}
}
