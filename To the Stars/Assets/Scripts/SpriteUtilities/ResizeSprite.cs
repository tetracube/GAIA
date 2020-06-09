using UnityEngine;
using System.Collections;

/// <summary>
///  Resizes a sprite to fill a given percentage of the screen.
/// </summary>
public class ResizeSprite : MonoBehaviour
{
    public SpriteRenderer m_sprite;           // Sprite to scale. If null it will take the one on this object
	public bool           m_keepProportions; 
	public bool           m_resizeVertial;

	public Camera         m_camera = null;    // Camera to take as referece for resizing

    [Range(0, 1)]
    public float m_screenPercentage = 1.0f;

	void Start ()
    {
		if(m_camera == null)
		{
			m_camera = Camera.main;
		}

		Sprite sprite;
        if (m_sprite == null)
        {
            sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }
		else
		{
			sprite = m_sprite.sprite;
		}

        // Resize the sprite according to its height
		if(m_resizeVertial)
		{
            // Calculate size based on the height of the screen
            float yFactorHeight = (GetScreenPos.CamHalfHeight(m_camera) * 2 / sprite.bounds.size.y) * m_screenPercentage;
            float xSizeHeight = transform.localScale.x + yFactorHeight - transform.localScale.y;

			if(m_keepProportions)
			{
				transform.localScale = new Vector3(xSizeHeight, yFactorHeight, 1);
			}
			else
			{
				transform.localScale = new Vector3(transform.localScale.x, yFactorHeight , 1);
			}
		}

        // Resize the sprite according to its width
		else
		{
            // Calculate size based on the width of the screen
            float xFactorWidth = (GetScreenPos.CamHalfWidth(m_camera) * 2 / sprite.bounds.size.x) * m_screenPercentage;
            float ySizeWidth = transform.localScale.y + xFactorWidth - transform.localScale.x;

			if(m_keepProportions)
			{
				transform.localScale = new Vector3(xFactorWidth, ySizeWidth, 1);
			}
			else
			{
				transform.localScale = new Vector3(xFactorWidth, transform.localScale.y, 1);
			}
		}
	}
}
