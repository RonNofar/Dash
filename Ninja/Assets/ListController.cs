using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour {

    public Sprite[] UpgradeImages;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    ArrayList Upgrades;

    private void Start()
    {
        SaveLoad.Load();

        Debug.Log("?");
        Upgrades = new ArrayList()
        {
            new Upgrade(
                UpgradeImages[0],
                "Velocity", 
                Upgrade.Type.VELOCITY),
            new Upgrade(
                UpgradeImages[1],
                "Health",
                Upgrade.Type.HEALTH),
            new Upgrade(
                UpgradeImages[2],
                "Coin",
                Upgrade.Type.COIN),
            new Upgrade(
                UpgradeImages[3],
                "Time",
                Upgrade.Type.TIME),
            new Upgrade(
                UpgradeImages[4],
                "Que",
                Upgrade.Type.QUE)

        };

        // 2. Iterate through the data, 
        //	  instantiate prefab, 
        //	  set the data, 
        //	  add it to panel
        foreach (Upgrade upgrade in Upgrades)
        {
            GameObject newUpgrade = Instantiate(ListItemPrefab);
            ListItemController controller = newUpgrade.GetComponent<ListItemController>();
            controller.Icon.sprite = upgrade.Icon;
            controller.Name.text = upgrade.Name;
            controller.level = upgrade.Level;
            controller.Level.text = controller.level + "";
            controller.cost = Upgrade.CalculateCost(upgrade.Level);
            controller.Cost.text = controller.cost + "";
            controller.type = upgrade.type;
            newUpgrade.transform.SetParent(ContentPanel.transform);
            newUpgrade.transform.localScale = Vector3.one;
        }
    }
}
