//Tags that identify data of XML file
public static class Utils
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
}
