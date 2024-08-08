using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    /// <summary>
    /// Object info used for planets, moons, stars and dwarf planets
    /// </summary>

    public BodyType bodyType;
    public PlanetType planetType;

    [SerializeField]
    private bool useCustomColor = false;
    [SerializeField]
    private Color customColor;
    [SerializeField]
    private Renderer crust;

    public BodyStatusType status = BodyStatusType.Inhabitable;

    public ColonyStatus colonyStatus;

    public Orbiting orbiting;

    public Sprite background;

    public void Start()
    {
        orbiting = GetComponent<Orbiting>();

        if(useCustomColor)
        {
            Material material = new Material(crust.material);
            material.color = customColor;
            crust.material = material;
        }
    }
    public override string GetDescription()
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
    public override void SetStatus(ref TMP_Text status)
    {
        switch (this.status)
        {
            case BodyStatusType.Colonized:
                status.text = "Colonized";
                status.color = ColorManager.instance.colonized;
                break;
            case BodyStatusType.Colonizable:
                status.text = "Colonizable";
                status.color = ColorManager.instance.colonizable;
                break;
            case BodyStatusType.CanBeTerraformed:
                status.text = "Can be terraformed";
                status.color = ColorManager.instance.canBeTerraformed;
                break;
            case BodyStatusType.Specialized:
                status.text = "Specialized";
                status.color = ColorManager.instance.specialized;
                break;
            case BodyStatusType.CanBeSpecialized:
                status.text = "Can be specialized";
                status.color = ColorManager.instance.canBeSpecialized;
                break;
            case BodyStatusType.Inhabitable:
                status.text = "Inhabitable";
                status.color = ColorManager.instance.inhabitable;
                break;
            default:
                status.text = "";
                break;
        }
    }

    public override void SpecialButtonClicked()
    {
        PlanetModalManager.Instance.Spawn(this);
    }

    public void AddFleet(Fleet fleet)
    {
        if(fleet is FriendlyFleet)
        {
            protectors.Add(fleet);
        }
        else
        {
            attackers.Add(fleet);
        }
        CheckForBattle();
    }

    public void RemoveFleet(Fleet fleet)
    {
        fleet.onOrbit = null;
        fleet.transform.SetParent(null);

        if (fleet is FriendlyFleet)
        {
            protectors.Remove(fleet);
        }
        else
        {
            attackers.Remove(fleet);
        }
    }
    public void CheckForBattle()
    {
        if(attackers.Count > 0 && protectors.Count > 0)
        {
            FriendlyFleet protector = protectors[0] as FriendlyFleet;
            EnemyFleet attacker = attackers[0] as EnemyFleet;

            BattlesManager.instance.AddBattle(protector, attacker);

            RemoveFleet(protector);
            RemoveFleet(attacker);
        }
    }
}
