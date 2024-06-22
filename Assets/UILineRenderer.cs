using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    // Script from https://www.youtube.com/watch?v=--LB7URk60A&t=1s by Game Dev Guide
    // Edited by me to bevel the edges of the line segments, and ensured that the thickness is uniform

    public Vector2Int gridSize;
    public List<Vector2> points = new List<Vector2>();
    public UIGridRenderer uiGrid;

    public float thickness = 10f;
    public bool center = true;

    float width;
    float height;
    float unitWidth;
    float unitHeight;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2 || gridSize.x < 2)
            return;

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / gridSize.x;
        unitHeight = height / gridSize.y;

        

        float angle = 0;

        for (int i = 0; i < points.Count - 1; i++)
        {
            // Create a line segment between the next two points
            CreateLineSegment(points[i], points[i + 1], vh, angle);

            int index = i * 5;

            // Add the line segment to the triangles array
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);

            // These two triangles create the beveled edges
            if (i != 0)
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }

    private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh, float angle)
    {
        Vector3 offset = center ? (rectTransform.sizeDelta / 2) : Vector2.zero;

        // Create vertex template
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector3 scaledPoint1 = new Vector3(point1.x * unitWidth, point1.y * unitHeight);
        Vector3 scaledPoint2 = new Vector3(point2.x * unitWidth, point2.y * unitHeight);

        // Create the start of the segment
        Quaternion point1Rotation = Quaternion.Euler(0, 0, GetAngle(point1, point2) + 90);
        vertex.position = point1Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += scaledPoint1;
        vh.AddVert(vertex);
        vertex.position = point1Rotation * new Vector3(thickness / 2, 0);
        vertex.position += scaledPoint1;
        vh.AddVert(vertex);

        // Create the end of the segment
        Quaternion point2Rotation = Quaternion.Euler(0, 0, GetAngle(point2, point1) - 90);
        vertex.position = point2Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += scaledPoint2;
        vh.AddVert(vertex);
        vertex.position = point2Rotation * new Vector3(thickness / 2, 0);
        vertex.position += scaledPoint2;
        vh.AddVert(vertex);

        // Also add the end point
        vertex.position = scaledPoint2;
        vh.AddVert(vertex);
    }

    private float GetAngle(Vector2 vertex, Vector2 target)
    {
        return Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * 180 / Mathf.PI;
    }

    void Update()
    {
        if (uiGrid != null)
        {
            if (gridSize != uiGrid.gridSize)
            {
                gridSize = uiGrid.gridSize;
                SetVerticesDirty();
            }
        }
    }
}
