//TEMPLATE TO BUILD TAGS.CS FILE -> Delete "_Template" to use this class or use another called TAGS.CS

using System.Collections;

//Tags that identify data of XML file
// <summary>
// Tags class. THIS is a template of Tags.cs class
// </summary>
// <remarks></remarks>
public static class Tags_Template
{
	//FSM type TAGS (DO NOT change or delete) 
	//RESERVED TAGS
    //<summary>RESERVED tag for FA_Classic. Do not delete</summary>
	public const int CLASSIC			= 0;
    //<summary>RESERVED tag for FA_Inertial. Do not delete</summary>
	public const int INERTIAL			= 1;
    //<summary>RESERVED tag for FA_Stack. Do not delete</summary>
	public const int STACK_BASED		= 2;
    //<summary>RESERVED tag for FA_Concurrent_States. Do not delete</summary>
	public const int CONCURRENT_STATES	= 3;
    //<summary>RESERVED tag for UNKNOWN entities. Do not delete or repeat</summary>
	public const int UNKNOWN 			= 1000;
		
	
	//Add states tags here (value >= 0, do not repeat a value)
	//...

	//Add transitions tags here (value >= 0, do not repeat a value)
	//...

	//Add actions tags here (value >= 0, do not repeat a value)
	//...

	//Add events tags here (value >= 0, do not repeat a value)
	//...

    // <summary>
    // Static method to define matching between const tags and strings
    // </summary>
    // <param name="word">An string to do the matching</param>
    // <returns>An integer tag value</returns>
    // <remarks>The user has to fill this method</remarks>
	public static int StringToTag (string word)
	{
		switch (word[0]) {
		case 'A':
			break; 
		case 'B':
			break;
		case 'C':
			if(word.Equals("CLASSIC")) 				return Tags.CLASSIC; 			//DO NOT DELETE
			if(word.Equals("CONCURRENT_STATES")) 	return Tags.CONCURRENT_STATES; 	//DO NOT DELETE
			break;
		case 'D':
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
			if(word.Equals("INERTIAL"))				return Tags.INERTIAL;			//DO NOT DELETE
			break;
		case 'J':
			break;
		case 'K':
			break;
		case 'L':
			break;
		case 'M':
			break;
		case 'N':
			if(word.Equals("NULL")) 				return Tags.UNKNOWN;			//DO NOT DELETE
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
			if(word.Equals("STACK_BASED")) 			return Tags.STACK_BASED;		//DO NOT DELETE
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
