using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public float frequency = 0.5f;

    public int FramesPerSec { get; protected set; }

    //public Text fpsText;

	void Awake()
	{
		//Application.targetFrameRate = 60;
	}

    private void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            //fpsText.text = FramesPerSec.ToString() + " fps";
        }
    }

	void OnGUI()
	{
		GUI.Label(new Rect(10, 40, 20, 20), FramesPerSec.ToString());
	}
}