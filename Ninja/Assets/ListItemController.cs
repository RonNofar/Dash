using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ListItemController : MonoBehaviour {

    public Image Icon;
    public Text Name, Level;
    public Text Cost;

    [HideInInspector]
    public Upgrade.Type type;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public int cost;

    public void IncrementUpgrade()
    {
        if (SaveData.current.coins >= cost)
        {
            ++level;

            SaveData.current.upgrades[type] = level;
            Level.text = level + "";

            SaveData.current.coins -= cost;

            cost = Upgrade.CalculateCost(level);
            Cost.text = cost + "";

            GUIManager.Instance.storePanelHandler.SetCoinText(SaveData.current.coins);
        }
    }
}
