using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONUtils : MonoBehaviour
{
    public static bool textureNotFoundWarning = false;

    public static Sprite LoadSprite(string folder, string fileName)
    {
        string path = "GFX/" + folder + fileName;

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
        {
            if (textureNotFoundWarning)
                Debug.LogWarning("Texture not found at path: " + path);
            return null;
        }

        return sprite;
    }

    public static Dictionary<string, int> StringIntToDictionary(string[] effectsString)
    {
        Dictionary<string, int> effects = new Dictionary<string, int>();

        foreach (string effect in effectsString)
        {
            string[] effectSplit = effect.Split(':');
            if (effectSplit.Length == 2)
                effects.Add(effectSplit[0], int.Parse(effectSplit[1]));
            else
                effects.Add(effectSplit[0], 0);
        }

        return effects;
    }
}
