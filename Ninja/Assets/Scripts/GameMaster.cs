using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    static protected GameMaster _instance;
    static public GameMaster Instance { get { return _instance; } }

    [HideInInspector]
    public bool isPaused;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Game Manager is already in play. Deleting new!", gameObject);
            Destroy(gameObject);
        }
        else
        { _instance = this; }
    }

    public void Pause(bool pause)
    {
        isPaused = pause;
        SetTimeScale(pause ? 0f : 1f);
    }

    private void SetTimeScale(float amount)
    {
        Time.timeScale = amount;
    }
}
