using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {

    static protected GUIManager _instance;
    static public GUIManager Instance { get { return _instance; } }

    [Header("Panels")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] GameObject PreGamePanel;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject GameOverPanel;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("GUI Manager is already in play. Deleting new!", gameObject);
            Destroy(gameObject);
        }
        else
        { _instance = this; }
    }

    private void Start ()
    {
        StartPanel.SetActive(true);
        PreGamePanel.SetActive(false);
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(false);

        GameMaster.Instance.Pause(true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void StartButton()
    {
        StartPanel.SetActive(false);
        PreGamePanel.SetActive(true);
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void EndPreGame()
    {
        StartPanel.SetActive(false);
        PreGamePanel.SetActive(false);
        GamePanel.SetActive(true);
        GameOverPanel.SetActive(false);

        GameMaster.Instance.Pause(false);
    }

    public void GameOver()
    {
        StartPanel.SetActive(false);
        PreGamePanel.SetActive(false);
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);

        GameMaster.Instance.Pause(true);

        StartCoroutine(Ninja.Util.Func.WaitAndRunActionInRealTime(
            1f, 
            () => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }));
    }
}
