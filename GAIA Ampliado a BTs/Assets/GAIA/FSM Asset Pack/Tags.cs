using System;
using System.Collections;
using System.Runtime.CompilerServices;

//Tags that identify data of XML file
public static class Tags
{
	//State tags
	public enum StateTags
	{
		NULL,
		MOVIENDO,
		DISPARANDO,
		APARTANDO
	}

	// <summary>
	// Get a string that has the name of a given State and returns the type of State associated
	// </summary>
	// <returns>StateTags</returns>
	// <remarks> Lexical analyzer. Converts a lexeme into a tag with meaning </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static StateTags stateName2Tag(string s)
	{
		StateTags type;

		if (Enum.TryParse(s, out type))
			return type;
		else return StateTags.NULL;
	}

	//Transition tags
	public enum TransitionTags
	{
		NULL,
		MOVIENDO_A_APUNTANDO,
		DISPARANDO_A_MOVIENDO,
		DISPARANDO_A_APUNTANDO,
		MOVIENDO_A_APARTANDO,
		DISPARANDO_A_APARTANDO,
		APARTANDO_A_DISPARANDO
	}

	// <summary>
	// Get a string that has the name of a given Transition and returns the type of Transition associated
	// </summary>
	// <returns>TransitionTags</returns>
	// <remarks> Lexical analyzer. Converts a lexeme into a tag with meaning </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TransitionTags transitionName2Tag(string s)
	{
		TransitionTags type;

		if (Enum.TryParse(s, out type))
			return type;
		else return TransitionTags.NULL;
	}   
	
	//EVENT TAGS
	public enum EventTags
	{
		NULL,
		DISTANCIA_OK,
		DISTANCIA_NO_OK,
		DEMASIADO_CERCA
	}

	// <summary>
	// Get a string that has the name of a given Event and returns the type of Event associated
	// </summary>
	// <returns>EventTags</returns>
	// <remarks> Lexical analyzer. Converts a lexeme into a tag with meaning </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EventTags eventName2Tag(string s)
	{
		EventTags type;

		if (Enum.TryParse(s, out type))
			return type;
		else return EventTags.NULL;
	}

	//ACTION TAGS
	public enum ActionTags
	{
		NULL,
		MOVERSE,
		DISPARAR,
		APARTAR
	}

	// <summary>
	// Get a string that has the name of a given Action of and returns the type of Action associated
	// </summary>
	// <returns>ActionTags</returns>
	// <remarks> Lexical analyzer. Converts a lexeme into a tag with meaning </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ActionTags actionName2Tag(string s)
	{
		ActionTags type;

		if (Enum.TryParse(s, out type))
			return type;
		else return ActionTags.NULL;
	}
}
