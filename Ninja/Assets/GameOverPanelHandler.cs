using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelHandler : MonoBehaviour {

    [SerializeField] Text dynamicCoinText;

    public void SetCoinText()
    {
        int coins = TestPlayer.Instance.coins;
        if (coins <= 0)
        {
            dynamicCoinText.gameObject.SetActive(false);
        }
        else if (coins > 0)
        {
            dynamicCoinText.gameObject.SetActive(true);
            dynamicCoinText.text = "+" + coins + " Coin" + (coins > 1 ? "s" : "");

        }
    }
}
