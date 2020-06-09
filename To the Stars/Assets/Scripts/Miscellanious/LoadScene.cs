using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour 
{
    public void LoadLevel(string scene)
    {
        Application.LoadLevel(scene);
    }

    public void LoadLastLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
