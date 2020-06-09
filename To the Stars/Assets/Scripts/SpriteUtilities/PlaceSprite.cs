using UnityEngine;
using System.Collections;

/// <summary>
/// It places the sprite at the chosen edge of the screen
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
public class PlaceSprite : MonoBehaviour 
{
    private SpriteRenderer m_sprite;

    public Position        m_pos;
	public float           m_percentageVisible = 0;

	public Camera          m_camera = null; // Camera to take as referece for positions

	void Start () 
    {
		if(m_camera == null)
		{
			m_camera = Camera.main;
		}

        m_sprite = gameObject.GetComponent<SpriteRenderer>();
        Place();
	} 
	 
    private void Place()
    {
        Vector3 offset; 

		if(m_pos == Position.Right)
		{
			transform.position = new Vector3(GetScreenPos.RightCenter(m_camera).x, GetScreenPos.RightCenter(m_camera).y, transform.position.z);
			offset = (Vector3.left * m_sprite.bounds.size.x) * m_percentageVisible;
			transform.Translate(Vector3.right * m_sprite.bounds.size.x / 2 + offset, Space.World);
		}
		else if(m_pos == Position.Left)
		{
			transform.position = new Vector3(GetScreenPos.LeftCenter(m_camera).x, GetScreenPos.LeftCenter(m_camera).y, transform.position.z);
		    offset = (Vector3.right * m_sprite.bounds.size.x) * m_percentageVisible;
			transform.Translate(Vector3.left * m_sprite.bounds.size.x / 2 + offset, Space.World);
		}
        else if (m_pos == Position.Bottom)
        {
			transform.position = new Vector3(GetScreenPos.BottomCenter(m_camera).x, GetScreenPos.BottomCenter(m_camera).y, transform.position.z);
			offset = (Vector3.up * m_sprite.bounds.size.y) * m_percentageVisible;
			transform.Translate(Vector3.down * m_sprite.bounds.size.y / 2 + offset, Space.World);
        }
		else if(m_pos == Position.Top)
        {
			transform.position = new Vector3(GetScreenPos.TopCenter(m_camera).x, GetScreenPos.TopCenter(m_camera).y, transform.position.z);
			offset = (Vector3.down * m_sprite.bounds.size.y) * m_percentageVisible;
			transform.Translate(Vector3.up * m_sprite.bounds.size.y / 2 + offset, Space.World);
        }
		else if (m_pos == Position.TopLeft)
		{
			transform.position = new Vector3(GetScreenPos.TopLeft(m_camera).x, GetScreenPos.TopLeft(m_camera).y, transform.position.z);
			offset = (Vector3.down * m_sprite.bounds.size.y) * m_percentageVisible;
			transform.Translate(Vector3.up * m_sprite.bounds.size.y / 2 + offset, Space.World);

			offset = (Vector3.right * m_sprite.bounds.size.x) * m_percentageVisible;
			transform.Translate(Vector3.left * m_sprite.bounds.size.x / 2 + offset, Space.World);
		}
		else if(m_pos == Position.BottomRight)
		{
			transform.position = new Vector3(GetScreenPos.BottomRight(m_camera).x, GetScreenPos.BottomRight(m_camera).y, transform.position.z);
			offset = (Vector3.up * m_sprite.bounds.size.y) * m_percentageVisible;
			transform.Translate(Vector3.down * m_sprite.bounds.size.y / 2 + offset, Space.World);
				
			offset = (Vector3.left * m_sprite.bounds.size.x) * m_percentageVisible;
			transform.Translate(Vector3.right * m_sprite.bounds.size.x / 2 + offset, Space.World);
		}
		else if(m_pos == Position.Center)
		{
            offset = (Vector3.up * m_sprite.bounds.size.y / 2) * m_percentageVisible;

			transform.position = new Vector3(GetScreenPos.Center(m_camera).x, GetScreenPos.Center(m_camera).y + offset.y, transform.position.z);
		}
		else if(m_pos == Position.CenterHalfTop)
		{
			transform.position = new Vector3(GetScreenPos.CenterHalfTop(m_camera).x, GetScreenPos.CenterHalfTop(m_camera).y, transform.position.z);
		}
		else if(m_pos == Position.CenterHalfBottom)
		{
			transform.position = new Vector3(GetScreenPos.CenterHalfBottom(m_camera).x, GetScreenPos.CenterHalfBottom(m_camera).y, transform.position.z);
		}
    }
}
