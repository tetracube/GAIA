using UnityEngine;
using System.Collections;

/// <summary>
/// To make it work create an empty object and place this script in it. The background sprites should be children of this object
/// It takes whatever number of sprites (>= 2) and keeps them scrolling at the same place.
/// So once one of the sprites has scrolled, it will place itself at the end of the las sprite to appear again.
/// This class also takes the sprites at the start and place them correctly one after another.
/// </summary>
public class Scrolling2D : MonoBehaviour 
{
    public SpriteRenderer[] m_scrollSprites;
    public float            m_speed = 10;
    
    private Transform       m_transform;
    private float           m_dinstanceMoved = 0;
    private int             m_firstSprite    = 0; // The sprite from the list being drawn on first place at the moment

	void Start ()
    {
        m_transform = transform;

        // Place each sprite after the first one, one ofter another
        for (int i = 1; i < m_scrollSprites.Length; i++)
        {
            PlaceSpriteAfterAnother(m_scrollSprites[i], m_scrollSprites[i-1]);
        }
	}

    /// <summary>
    /// It places a sprite right after another
    /// </summary>
    /// <param name="spriteToPlace">Sprite which is going to be placed after the reference sprite</param>
    /// <param name="refSprite">Reference sprite</param>
    void PlaceSpriteAfterAnother(SpriteRenderer spriteToPlace, SpriteRenderer refSprite)
    {
        spriteToPlace.transform.position = new Vector3(refSprite.transform.position.x + refSprite.bounds.size.x / 2 + spriteToPlace.bounds.size.x / 2,
                                                       refSprite.transform.position.y,
                                                       refSprite.transform.position.z);

    }

	
	void Update ()
    {
        float distanceToMove = Time.deltaTime * m_speed;

        // At the end of the entire sprite's movement
        if (m_dinstanceMoved + distanceToMove > m_scrollSprites[m_firstSprite].bounds.size.x)
        {
            // Limit the total movement of the sprite to it's sprite's size. To avoid undesired offsets
            distanceToMove = m_scrollSprites[m_firstSprite].bounds.size.x - m_dinstanceMoved;
        }

        // Move background
        m_transform.Translate(Vector3.left * distanceToMove);
        m_dinstanceMoved += distanceToMove;

        // If the first sprite has completely moved, place it at the end of the queue of sprites
        if (m_dinstanceMoved == m_scrollSprites[m_firstSprite].bounds.size.x)
        {
            int lastSprite = (m_firstSprite - 1 + m_scrollSprites.Length) % m_scrollSprites.Length;
            PlaceSpriteAfterAnother(m_scrollSprites[m_firstSprite], m_scrollSprites[lastSprite]);
            m_firstSprite = (m_firstSprite + 1) % m_scrollSprites.Length;
            m_dinstanceMoved = 0;
        }

        
	}
}
