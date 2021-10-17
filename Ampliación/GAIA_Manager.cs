//Uncoment the line below to enable the use of the BT functions.
#define PANDA

using System.Collections.Generic;
using System;
using UnityEngine;
using Panda;
using GAIA;

namespace GAIA
{
    // Manager of FSMs and BTs
    public class GAIA_Manager
    {


        //GAIA_Manager Class Attributes
        // An instance of an GAIA_Parser (filled with GAIA_Manager(parser) constructor.
        xmltest.GAIA_Parser parser;
        //Dictionary to add a finite automata with FAtype + FAid (Tuple) and the FA itself
        Dictionary<Tuple, FA_Classic> FSM_dic;
        //Dictionary to add a behaviour tree with the BT definition file name and its contents
        Dictionary<string, string> BT_dic;

        // Initializes a new instance of the GAIA_Manager class with an instance of GAIA_Parser.
        public GAIA_Manager(xmltest.GAIA_Parser parser)
        {
            this.parser = parser;
            this.FSM_dic = new Dictionary<Tuple, FA_Classic>();
            this.BT_dic = new Dictionary<string, string>();
        }

        // Initializes a new instance of the GAIA_Manager class.
        public GAIA_Manager()
        {
            this.FSM_dic = new Dictionary<Tuple, FA_Classic>();
            this.BT_dic = new Dictionary<string, string>();
        }

        //Struct that allows to add a Finite automata with type+ID
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

        // Add a machine (passed as FA parameter) to this GAIA_Manager
        // Returns:
        // 1 if OK
        //-1 if cannot be added
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


        // Adds a machine (located in path) to this GAIA_Manager
        // Returns:
        // 1 if OK
        //-1 if an GAIA_Parser is required to use this method
        //-2 if cannot be added (wrong parameters or repeated FA)
        //-3 if there are errors parsing file located in path
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

#if (PANDA)
        // Add a behaviour tree (passed as the contents of an xml file in the form of a string) to this GAIA_Manager
        // Returns:
        // 1 if OK
        //-1 if parser is null
        //-2 if there is a problem adding the BT into the dictionary
        //-3 if the contents returned by the parser are null

        public int addBT(string content)
        {
            string[] parsedbt = new string[1];

            if (this.parser == null)
            {
                return -1;
            }
            else
            {
                if(content != null)
                { //Invoke parser with string
                    parsedbt = parser.ParseBT(content);
                }
                if (parsedbt != null)
                {
                    try
                    {
                        BT_dic.Add(parsedbt[0], parsedbt[1]);
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
#endif

        // Removes a machine that had been added to this GAIA_Manager
        // Returns:
        // 1 if OK
        //-1 if cannot remove that machine (corrupted fsm object or it does not exist)
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

#if (PANDA)
        // Removes a behaviour tree that had been added to this GAIA_Manager
        // Returns:
        // 1 if OK
        //-1 if cannot remove that behaviour tree (corrupted BT object or it does not exist)
        public int deleteBT(string bt_id)
        {
            try
            {
                BT_dic.Remove(bt_id);
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
#endif

        // Creates a returnable FSM_Machine to an entity (character)
        // Returns:
        // The correct value of FSM_Machine or NULL (wrong parameters or that FSM does not exist))
        // It tries to find a matching between the passed parameters and an instance in the internal FSM Dictionary (FSM_Dic)
        public FSM_Machine createMachine(System.Object character, int FSM_type, string FSM_id)
        {
            try
            {
                return new FSM_Machine(FSM_dic[new Tuple(FSM_type, FSM_id)], character);
            }
            catch (Exception e)
            {
                Debug.Log("EXCEPTION: " + e);
                return null;
            }
        }

#if (PANDA)
        // Adds a PandaBehaviour component to the passed GameObject and compiles the selected BT based on the file name.
        // The file name servers as the key in the dictionary to retrieve the BT definition.
        public void createBT(GameObject character, string bt_id)
        {
            try
            {
                PandaBehaviour component = character.AddComponent<PandaBehaviour>();
                component.Compile(BT_dic[bt_id]);
            }
            catch (Exception e)
            {
                Debug.Log("EXCEPTION: " + e);
            }
        }
#endif

#if (PANDA)
        // Change the BT to run by the PandaBehaviour component.
        // The file name servers as the key in the dictionary to retrieve the BT definition.
        public void changeBT(GameObject character, string bt_id)
        {
            try
            {
                PandaBehaviour component = character.GetComponent<PandaBehaviour>();
                component.Compile(BT_dic[bt_id]);
            }
            catch (Exception e)
            {
                Debug.Log("EXCEPTION: " + e);
            }
        }
#endif
    }
}