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
                SetLineFormation(composition, capitan);
                break;
        }
    }
    public void SetLineFormation(Ship[] composition, GameObject capitan)
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
                composition[i].flyPattern.myOffset = Vector3.zero;
                continue;
            }
            if(itemInRow < row)
            {
                composition[i].flyPattern.myOffset = offset[itemInRow];
                itemInRow++;
                continue;
            }

            itemInRow = 0;
            row++;
            offset = OffsetForRow(row, distanceX, distanceZ);
            composition[i].flyPattern.myOffset = offset[itemInRow];
            itemInRow++;

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
