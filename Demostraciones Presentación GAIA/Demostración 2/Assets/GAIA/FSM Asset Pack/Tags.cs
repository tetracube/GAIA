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
	public const int MOVIENDO	      	= 0;
    public const int DISPARANDO         = 1;
	public const int APARTANDO			= 2;

	//Transition tags
	public const int MOVIENDO_A_APUNTANDO	 = 0;
	public const int DISPARANDO_A_MOVIENDO	 = 1;
	public const int DISPARANDO_A_APUNTANDO	 = 2;
	public const int MOVIENDO_A_APARTANDO	 = 3;
	public const int DISPARANDO_A_APARTANDO	 = 4;
	public const int APARTANDO_A_DISPARANDO	 = 5;

	//EVENT TAGS
	public const int DISTANCIA_OK	 = 0;
    public const int DISTANCIA_NO_OK = 1;
	public const int DEMASIADO_CERCA = 2;

	//ACTION TAGS
	public const int MOVERSE 	 = 0;
    public const int DISPARAR	 = 1;
    public const int APARTAR	 = 2;

	//add more
	public static int StringToTag (string word)
    {
			//if(word.Equals("NULL")) return 49;// UnityEngine.Debug.Log("Soy null");
			switch (word[0]) {
			case 'A':
				if (word.Equals("APARTANDO")) return Tags.APARTANDO;
				if (word.Equals("APARTANDO_A_DISPARANDO")) return Tags.APARTANDO_A_DISPARANDO;
				if (word.Equals("APARTAR")) return Tags.APARTAR;
				break; 
			case 'B':
				break;
			case 'C':
				if(word.Equals("CLASSIC"))           return Tags.CLASSIC; 
				if(word.Equals("CONCURRENT_STATES")) return Tags.CONCURRENT_STATES; //DO NOT DELETE
				break;
			case 'D':
				if (word.Equals("DISPARANDO")) return Tags.DISPARANDO;
				if (word.Equals("DISPARANDO_A_MOVIENDO")) return Tags.DISPARANDO_A_MOVIENDO;
				if (word.Equals("DISPARANDO_A_APUNTANDO")) return Tags.DISPARANDO_A_APUNTANDO;
				if (word.Equals("DISPARANDO_A_APARTANDO")) return Tags.DISPARANDO_A_APARTANDO;
				if (word.Equals("DISPARAR")) return Tags.DISPARAR;
				if (word.Equals("DISTANCIA_OK")) return Tags.DISTANCIA_OK;
				if (word.Equals("DISTANCIA_NO_OK")) return Tags.DISTANCIA_NO_OK;
				if (word.Equals("DEMASIADO_CERCA")) return Tags.DEMASIADO_CERCA;
				break;
			case 'E':
				break;
			case 'F':
				break;
			case 'G':
				break;
			case 'H':
				break;
			case 'I':
				if(word.Equals("INERTIAL"))	return Tags.INERTIAL;
				break;
			case 'J':
				break;
			case 'K':
				break;
			case 'L':
				break;
			case 'M':
				if (word.Equals("MOVIENDO")) return Tags.MOVIENDO;
				if (word.Equals("MOVERSE")) return Tags.MOVERSE;
				if (word.Equals("MOVIENDO_A_APUNTANDO")) return Tags.MOVIENDO_A_APUNTANDO;
				if (word.Equals("MOVIENDO_A_APARTANDO")) return Tags.MOVIENDO_A_APARTANDO;
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
                if (word.Equals("STACK_BASED"))        return Tags.STACK_BASED;
				break;
			case 'T':
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
