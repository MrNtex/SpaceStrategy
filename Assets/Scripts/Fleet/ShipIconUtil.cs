using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipIconUtil : MonoBehaviour
{
    [System.Serializable]
    public struct ShipIcon
    {
        public ShipType type;
        public Sprite icon;
    } // Dictionary in the inspector is not serializable, so this is a workaround

    [SerializeField]
    private List<ShipIcon> icons = new List<ShipIcon>();

    public static Dictionary<ShipType, Sprite> shipIcons = new Dictionary<ShipType, Sprite>();

    private void Awake()
    {
        foreach (ShipIcon icon in icons)
        {
            shipIcons.Add(icon.type, icon.icon);
        }
    }
}
