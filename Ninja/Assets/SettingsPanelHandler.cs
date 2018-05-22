using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelHandler : MonoBehaviour {

    #region Button Functions
    public void ResetStatsButton()
    {
        SaveData.current = new SaveData();
        SaveLoad.Save();

        GameMaster.ResetGame();
    }
    #endregion
}
