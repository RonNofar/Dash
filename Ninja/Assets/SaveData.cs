using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public static SaveData current;
    public int coins;
    public Dictionary<Upgrade.Type, int> upgrades;

    public SaveData()
    {
        upgrades = new Dictionary<Upgrade.Type, int>();
        upgrades.Add(Upgrade.Type.VELOCITY, 1);
        upgrades.Add(Upgrade.Type.HEALTH,   1);
        upgrades.Add(Upgrade.Type.COIN,     1);
        upgrades.Add(Upgrade.Type.TIME,     1);
        upgrades.Add(Upgrade.Type.QUE,      1);

        coins = 0;
    }
}
