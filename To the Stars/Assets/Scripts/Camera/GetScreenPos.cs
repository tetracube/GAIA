using UnityEngine;
using System.Collections;

/// <summary>
/// Returns screen positions for ortographic cameras. 
/// </summary>
public class GetScreenPos
{
	static public Vector2 LeftCenter(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x - CamHalfWidth(camera), cam.transform.position.y);
	}

	static public Vector2 RightCenter(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x + CamHalfWidth(camera), cam.transform.position.y);
	}

	static public Vector2 BottomCenter(Camera camera = null)
    {
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x, cam.transform.position.y - cam.orthographicSize);
    }

	static public Vector2 TopCenter(Camera camera = null)
    {
		Camera cam = (camera != null) ? camera : Camera.main;
        return new Vector3(cam.transform.position.x, cam.transform.position.y + cam.orthographicSize);
    }

	static public Vector2 TopLeft(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x - CamHalfWidth(camera), cam.transform.position.y + cam.orthographicSize);
	}

	static public Vector2 BottomRight(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x + CamHalfWidth(camera), cam.transform.position.y - cam.orthographicSize);
	}

	static public Vector2 Center(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x, cam.transform.position.y);
	}

	static public Vector2 CenterHalfTop(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x, cam.transform.position.y + CamHalfHeight(camera) / 2);
	}

	static public Vector2 CenterHalfBottom(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return new Vector3(cam.transform.position.x, cam.transform.position.y - CamHalfHeight(camera) / 2);
	}

	static public float CamHalfHeight(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return cam.orthographicSize;
	}

	static public float CamHalfWidth(Camera camera = null)
	{
		Camera cam = (camera != null) ? camera : Camera.main;
		return cam.aspect * cam.orthographicSize;
	}
	
}
