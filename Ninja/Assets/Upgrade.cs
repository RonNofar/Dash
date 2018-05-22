using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade {

    public Sprite Icon;
    public string Name;
    public int Level;

    [HideInInspector]
    public Upgrade.Type type;

    public enum Type
    {
        NULL     = -1,
        VELOCITY =  0, // travel velocity 
        HEALTH   =  1, // time pickup multiplier
        COIN     =  2, // coin pickup multiplier
        TIME     =  3, // initial time
        QUE      =  4  // total que-able clicks
    }

    public Upgrade(Sprite icon, string name, Type type)
    {
        Icon = icon;
        Name = name;
        Level = SaveData.current.upgrades[type];
        this.type = type;
    }

    public static int CalculateCost(int level)
    {
        return Mathf.RoundToInt(Mathf.Pow(5, level - 1)); // 5 is magic number :P
    }

    public static float CalculateVelocity(float initialVelocity, int level)
    {
        return (initialVelocity * (1 + ((level - 1) / 10)));
    }

    public static float CalculateHealth(float initialHealth, int level)
    {
        return (initialHealth * (1 + ((level - 1) / 10)));
    }

    public static int CalculateCoin(int initialCoin, int level)
    {
        return initialCoin * level;
    }
    
    public static float CalculateTime(int level)
    { // Only calculates the addition NOT total (different from others)
        return ((level - 1) / 2f);
    }

    public static int CalculateQue(int initialQueLength, int level)
    {
        return initialQueLength + level - 1;
    }
}
