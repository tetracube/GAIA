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

	//EVENT TAGS
	public enum EventTags
	{
		NULL,
		DISTANCIA_OK,
		DISTANCIA_NO_OK,
		DEMASIADO_CERCA
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
	// Get a string that has the name of a given enumeration and returns the type of enumerated value associated
	// </summary>
	// <returns>Generic enumerated value</returns>
	// <remarks> Generic lexical analyzer. Converts a lexeme into a tag with meaning </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TEnum name2Tag<TEnum>(string s)
	where TEnum : struct
	{
		TEnum resultInputType;

		Enum.TryParse(s, true, out resultInputType);
		return resultInputType;
	}
}
