using UnityEngine;
using System.Collections;

//Tags that identify data of XML file
public static class Tags
{
	//FSM type TAGS (DO NOT change or delete)
	public const int CLASSIC			= 0;
	public const int INERTIAL			= 1;	
	public const int STACK_BASED		= 2;	
	public const int CONCURRENT_STATES	= 3;
	public const int UNKNOWN 			= 1000;	//UNKNOWN

	//State tags
    public const int SAD_STAR    		= 0;	//State
	public const int HAPPY_STAR      	= 1;	//State
	public const int LOST_STAR	 		= 2;	//State
    public const int HIT_STAR           = 3;

	//Transition tags
	public const int TO_HAPPY_STAR      = 0;	//Transition
	public const int TO_LOST_STAR	    = 1;	//Transition
    public const int TO_SAD_STAR        = 2;
    public const int TO_HIT_STAR        = 3;

	//EVENT TAGS
	public const int EVENT_HAPPY_STAR	= 0;
	public const int EVENT_LOST_STAR   	= 1;
    public const int EVENT_SAD_STAR     = 2;
    public const int EVENT_HIT_STAR     = 3;

	//ACTION TAGS
    public const int SAD_STAR_ACTION      = 0;
	public const int HAPPY_STAR_ACTION 	  = 1;	
	public const int LOST_STAR_ACTION	  = 2;
    public const int LOST_STAR_IN_ACTION  = 3;
    public const int HAPPY_STAR_IN_ACTION = 4;
    public const int HIT_STAR_ACTION      = 5;
    public const int HIT_STAR_IN_ACTION   = 6;

	//add more
	public static int StringToTag (string word)
    {
			//if(word.Equals("NULL")) return 49;// UnityEngine.Debug.Log("Soy null");
			switch (word[0]) {
			case 'A':
				break; 
			case 'B':
				break;
			case 'C':
				if(word.Equals("CLASSIC"))           return Tags.CLASSIC; 
				if(word.Equals("CONCURRENT_STATES")) return Tags.CONCURRENT_STATES; //DO NOT DELETE
				break;
			case 'D':
				break;
			case 'E':
                if (word.Equals("EVENT_HAPPY_STAR")) return Tags.EVENT_HAPPY_STAR;
                if (word.Equals("EVENT_LOST_STAR"))  return Tags.EVENT_LOST_STAR;
                if (word.Equals("EVENT_SAD_STAR"))   return Tags.EVENT_SAD_STAR;
                if (word.Equals("EVENT_HIT_STAR"))   return Tags.EVENT_HIT_STAR;
				break;
			case 'F':
				break;
			case 'G':
				break;
			case 'H':
                if (word.Equals("HAPPY_STAR"))           return Tags.HAPPY_STAR;
                if (word.Equals("HAPPY_STAR_ACTION"))    return Tags.HAPPY_STAR_ACTION;
                if (word.Equals("HAPPY_STAR_IN_ACTION")) return Tags.HAPPY_STAR_IN_ACTION;
                if (word.Equals("HIT_STAR"))             return Tags.HIT_STAR;
                if (word.Equals("HIT_STAR_ACTION"))      return Tags.HIT_STAR_ACTION;
                if (word.Equals("HIT_STAR_IN_ACTION"))   return Tags.HIT_STAR_IN_ACTION;
				break;
			case 'I':
				if(word.Equals("INERTIAL"))	return Tags.INERTIAL;
				break;
			case 'J':
				break;
			case 'K':
				break;
			case 'L':
                if (word.Equals("LOST_STAR"))           return Tags.LOST_STAR;
                if (word.Equals("LOST_STAR_ACTION"))    return Tags.LOST_STAR_ACTION;
                if (word.Equals("LOST_STAR_IN_ACTION")) return Tags.LOST_STAR_IN_ACTION;
				break;
			case 'M':
				break;
			case 'N':
				if(word.Equals("NULL")) return Tags.UNKNOWN;
				break;
			case 'O':
				break;
			case 'P':				
				break;
			case 'Q':
				break;
			case 'R':
				break;
			case 'S':
                if (word.Equals("SAD_STAR"))           return Tags.SAD_STAR;
                if (word.Equals("SAD_STAR_ACTION"))    return Tags.SAD_STAR_ACTION;
                if (word.Equals("STACK_BASED"))        return Tags.STACK_BASED;
				break;
			case 'T':
                if (word.Equals("TO_HAPPY_STAR")) return Tags.TO_HAPPY_STAR;
                if (word.Equals("TO_LOST_STAR"))  return Tags.TO_LOST_STAR;
                if (word.Equals("TO_SAD_STAR"))   return Tags.TO_SAD_STAR;
                if (word.Equals("TO_HIT_STAR"))   return Tags.TO_HIT_STAR;
				break;
			case 'U':
				break;
			case 'V':
				break;
			case 'W':
				break;
			case 'X':
				break;
			case 'Y':
				break;
			case 'Z':
				break;						
			}
			return Tags.UNKNOWN;
		}
}
