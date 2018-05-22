using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanelHandler : MonoBehaviour {

    [SerializeField] RectTransform contentPanel;
    [SerializeField] Text coinText;

    private void OnEnable()
    {
        OnStoreEnter();
    }

    private void OnDisable()
    {
        OnStoreExit();
    }

    void OnStoreEnter()
    {
        SaveLoad.Load();
        SetCoinText(SaveData.current.coins);
        contentPanel.anchoredPosition = Vector2.zero;
    }

    void OnStoreExit()
    {
        SaveLoad.Save();
    }

    public void SetCoinText(int coins)
    {
        coinText.text = "Total Coins: " + coins;
    }
}
