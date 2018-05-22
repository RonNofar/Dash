using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    static protected TimeManager _instance;
    static public TimeManager Instance { get { return _instance; } }

    [SerializeField] GamePanelHandler gamePanel;
    [SerializeField] float initialTime = 1f;

    private float startTime;
    private float currentTime;

    [HideInInspector]
    public float timeLeft;

    private bool isDone;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning(gameObject.name+" is already in play. Deleting new!", gameObject);
            Destroy(gameObject);
        }
        else
        { _instance = this; }
    }

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer(float? initialTime = null)
    {
        startTime = Time.time;
        if (initialTime != null)
            this.initialTime = (float)initialTime;
    }

    public void AddTime(float time)
    {
        initialTime += time;
    }

    private void Update()
    {
        if (!isDone)
        {
            currentTime = Time.time - startTime;
            timeLeft = initialTime - currentTime;
            //Debug.Log("?");

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                isDone = true;

                GUIManager.Instance.GameOver();
            }

            //if (!isDone) Debug.Log(timeLeft);

            //gamePanel.SetTimeText("" + Mathf.RoundToInt(timeLeft * 1000f));
        }
    }


}
