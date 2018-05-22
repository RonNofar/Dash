using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelHandler : MonoBehaviour {

    [SerializeField] Text timeText;
    [SerializeField] Text coinText;

    private void Update()
    {
        SetCoinText(TestPlayer.Instance.coins+"");
        SetTimeText( Mathf.RoundToInt(
                TimeManager.Instance.timeLeft * 1000f)+"");
    }

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    public void SetCoinText(string text)
    {
        coinText.text = text;
    }
}
