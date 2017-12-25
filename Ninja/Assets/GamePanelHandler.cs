using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelHandler : MonoBehaviour {

    [SerializeField] Text timeText;
    [SerializeField] Text coinText;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }
}
