using UnityEngine;
using System.Collections;

/// <summary>
/// It draws an horizontal line of points on Unity's editor
/// </summary>
public class Lane : MonoBehaviour
{
    public LanePos m_lanePos;

    public int m_pointsToDraw          = 10;
    public int m_distanceBetweenPoints = 3;

    private void OnDrawGizmos()
    {
        // Draw the different points
        for (int i = 0; i < m_pointsToDraw; i++)
        {
            Gizmos.DrawIcon(new Vector3(transform.position.x + i * m_distanceBetweenPoints, transform.position.y, transform.position.z), "point.png", false);
        }

        // Draw a line from first point to last one
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x + (m_pointsToDraw - 1) * m_distanceBetweenPoints, transform.position.y, transform.position.z));
    }

}
