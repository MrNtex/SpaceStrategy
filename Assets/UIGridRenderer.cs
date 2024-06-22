using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIGridRenderer : Graphic
{
    // Script from https://www.youtube.com/watch?v=--LB7URk60A&t=1s by Game Dev Guide

    public Vector2Int gridSize = new Vector2Int(1, 1);

    public float thickness = 1;

    float width;
    float height;

    float cellWidth;
    float cellHeight;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        cellWidth = width / (float)gridSize.x;
        cellHeight = height / (float)gridSize.y;

        int count = 0;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                DrawCell(vh, x, y, count);
                count++;
            }
        }
    }

    private void DrawCell(VertexHelper vh, int x, int y, int index)
    {

        float xPos = x * cellWidth;
        float yPos = y * cellHeight;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(xPos, yPos);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos, yPos + cellHeight);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth, yPos + cellHeight);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth, yPos);
        vh.AddVert(vertex);

        float widthSqr = thickness * thickness;
        float distance = Mathf.Sqrt(widthSqr / 2);

        vertex.position = new Vector3(xPos + distance, yPos + distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + distance, yPos + cellHeight - distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth - distance, yPos + cellHeight - distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth - distance, yPos + distance);
        vh.AddVert(vertex);

        int offset = index * 8;

        // Left edge
        vh.AddTriangle(offset + 0, offset + 1, offset + 5);
        vh.AddTriangle(offset + 5, offset + 4, offset + 0);
        // Top edge
        vh.AddTriangle(offset + 1, offset + 2, offset + 6);
        vh.AddTriangle(offset + 6, offset + 5, offset + 1);
        // Right edge
        vh.AddTriangle(offset + 2, offset + 3, offset + 7);
        vh.AddTriangle(offset + 7, offset + 6, offset + 2);
        // Bottom edge
        vh.AddTriangle(offset + 3, offset + 0, offset + 4);
        vh.AddTriangle(offset + 4, offset + 7, offset + 3);
    }
}
