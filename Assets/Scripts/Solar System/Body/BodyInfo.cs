using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BodyInfo;
public enum BodyType
{
    Planet,
    Main, // The planet that determines the time scale (Earth)
    Star,
    Moon,
    DwarfPlanet
}
public enum PlanetType
{
    Terrestrial,
    GasGiant,
    IceGiant
}
public class BodyInfo : ObjectInfo
{
    public BodyType bodyType;
    public PlanetType planetType;
    public override string GetDescrition()
    {
        string description = "";   
        switch (bodyType)
        {
            case BodyType.Moon:
                description = "Moon of " + orbiting.target.name;
                break;
            case BodyType.DwarfPlanet:
                description = "Dwarf planet";
                break;
            case BodyType.Planet:
                switch (planetType)
                {
                    case PlanetType.Terrestrial:
                        description = "Terrestrial planet";
                        break;
                    case PlanetType.GasGiant:
                        description = "Gas giant";
                        break;
                    case PlanetType.IceGiant:
                        description = "Ice giant";
                        break;
                    default:
                        break;
                }
                break;
            case BodyType.Star:
                description = "Star";
                break;
            case BodyType.Main:
                description = "Main planet";
                break;
            default:
                break;
        }
        return description;
    }
    public override Sprite GetIcon()
    {
        switch (bodyType)
        {
            case BodyType.Moon:
                return SpriteManager.instance.moon;
            default:
                break;
        }
        return SpriteManager.instance.moon;
    }
    public override Color GetColor()
    {
        return ColorManager.instance.bodyColor[(int)bodyType];
    }
}
