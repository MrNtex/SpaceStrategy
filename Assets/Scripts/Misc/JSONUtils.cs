using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONUtils : MonoBehaviour
{
    public static Sprite LoadSprite(string folder, string fileName)
    {
        string path = "GFX/" + folder + fileName;

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
        {
            Debug.LogWarning("Texture not found at path: " + path);
            return null;
        }

        return sprite;
    }
}
