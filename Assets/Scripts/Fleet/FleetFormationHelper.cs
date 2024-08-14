using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetFormationHelper : MonoBehaviour
{
    public static FleetFormationHelper instance;
    private void Awake()
    {
        instance = this;
    }
    public void SetFormation(FleetFormation formation, Ship[] composition, GameObject capitan)
    {
        switch (formation)
        {
            case FleetFormation.Triangle:
                SetTriangleFormation(composition, capitan);
                break;
            case FleetFormation.Echelon:
                SetEchelonFormation(composition, capitan);
                break;
        }
    }
    public void SetTriangleFormation(Ship[] composition, GameObject capitan)
    {
        float distanceX = 3;
        float distanceZ = -7.5f;

        int row = 2;
        int itemInRow = 0;

        Vector3[] offset = OffsetForRow(row, distanceX, distanceZ);
        for (int i = 0; i < composition.Length; i++)
        {
            if (composition[i].prefab == capitan)
            {
                composition[i].myOffset = Vector3.zero;
                continue;
            }
            if(itemInRow < row)
            {
                composition[i].myOffset = offset[itemInRow];
                itemInRow++;
                continue;
            }

            itemInRow = 0;
            row++;
            offset = OffsetForRow(row, distanceX, distanceZ);
            composition[i].myOffset = offset[itemInRow];
            itemInRow++;

        }
    }
    public void SetEchelonFormation(Ship[] composition, GameObject capitan) // https://en.wikipedia.org/wiki/Echelon_formation
    {
        const int maxCol = 2;
        Vector3 constChange = new Vector3(3, .5f, 3);
        Vector3 offset = Vector3.zero;

        int row = 0;

        int itemInCol = 1;

        for (int i = 0; i < composition.Length; i++)
        {
            if (composition[i].prefab == capitan)
            {
                composition[i].myOffset = Vector3.zero;
                continue;
            }
            if(itemInCol > maxCol)
            {
                row++;
                itemInCol = 1;
                offset = new Vector3(0, offset.y + 1.5f, 5 * row);
            }

            offset = constChange + offset;
            composition[i].myOffset = offset;
            i++;
            if (i >= composition.Length) break;
            composition[i].myOffset = new Vector3(-offset.x, offset.y, offset.z);

            itemInCol++;

        }
    }
    private Vector3[] OffsetForRow(int row, float distanceX, float distanceZ)
    {
        Vector3[] offset = new Vector3[row];

        float startX = -(row - 1) * distanceX / 2;
        float z = (row - 1) * distanceZ;

        for (int i = 0; i < row; i++)
        {
            offset[i] = new Vector3(startX + i * distanceX, 0, z);
        }
        return offset;
    }
}
