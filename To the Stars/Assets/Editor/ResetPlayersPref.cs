using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

public class ResetPlayersPref : Editor 
{
    [MenuItem("ToTheStars/ClearPlayerprebs")]
    static void ClearPlayerPrefbs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif
