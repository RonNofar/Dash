using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GUIManager : MonoBehaviour {

    static protected GUIManager _instance;
    static public GUIManager Instance
    { // https://gist.github.com/simonbroggi/720ea3388ae10ead6771
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR
                GameObject o;
                if (Application.isPlaying)
                {
                    o = (GameObject)Instantiate(Resources.Load("GUIManager"));
                }
                else
                {
                    o = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GUIManager.prefab");
                    if (o == null)
                    {
                        GameObject tempGO = new GameObject("GUIManager", typeof(GUIManager));
                        o = UnityEditor.PrefabUtility.CreatePrefab("Assets/Resources/GUIManager.prefab", tempGO);
                        DestroyImmediate(tempGO);
                    }
                }
#else
				GameObject o = (GameObject)Instantiate(Resources.Load("ResourcesSingleton"));
#endif
                _instance = ((GameObject)o).GetComponent<GUIManager>();
            }
            return _instance;
        }
    }

    [Header("Panels")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] GameObject PreGamePanel;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject StorePanel;
    [SerializeField] GameObject SettingsPanel;

    [HideInInspector]
    public List<GameObject> panelList;

    [HideInInspector]
    public StorePanelHandler storePanelHandler;

    private bool isSettingsOpen = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning(_instance.name + " already exists! Destroying " + this.name);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void Start ()
    {
        OnStartPanel();

        PopulateListComplete();

        storePanelHandler = StorePanel.GetComponent<StorePanelHandler>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PopulateListComplete()
    { // to be used outside of GUIManager
        PopulateList(new GameObject[] {
            StartPanel    == null ? GameObject.Find("StartPanel")    : StartPanel,
            PreGamePanel  == null ? GameObject.Find("PreGamePanel")  : PreGamePanel,
            GamePanel     == null ? GameObject.Find("GamePanel")     : GamePanel,
            GameOverPanel == null ? GameObject.Find("GameOverPanel") : GameOverPanel,
            StorePanel    == null ? GameObject.Find("StorePanel")    : StorePanel,
            SettingsPanel == null ? GameObject.Find("SettingsPanel") : SettingsPanel
        });
    }

    private void PopulateList(GameObject[] goArr)
    {
        panelList = new List<GameObject>();
        for (int i = 0; i < goArr.Length; ++i)
        {
            panelList.Add(goArr[i]);
        }
    }

    public void TogglePanel(GameObject panel, GameObject[] exempt = null)
    {
        if (exempt == null)
        {
            exempt = new GameObject[] { };
        }
        panel.SetActive(true);
        foreach(GameObject go in panelList)
        {
            if (go != panel && !exempt.Contains(go))
            {
                go.SetActive(false);
            }
        }
    }

    private void OnStartPanel()
    {
        SaveLoad.Load();

        TogglePanel(StartPanel);

        GameMaster.Instance.Pause(true);
    }

    public void StartButton()
    {
        TogglePanel(PreGamePanel);
    }

    public void StoreButton(bool open)
    {
        if (open)
        {
            TogglePanel(StorePanel, new GameObject[] { StartPanel });
        }
        else
        {
            TogglePanel(StartPanel);
        }
    }

    public void SettingsButton()
    {
        if (!isSettingsOpen)
        {
            TogglePanel(SettingsPanel, new GameObject[] { StartPanel });
        }
        else
        {
            TogglePanel(StartPanel);
        }
        isSettingsOpen = !isSettingsOpen;
    }

    public void EndPreGame()
    {
        TogglePanel(GamePanel);

        GameMaster.Instance.Pause(false);
    }

    public void GameOver()
    {
        GameOverPanel.GetComponent<GameOverPanelHandler>().SetCoinText();

        TogglePanel(GameOverPanel);

        GameMaster.Instance.Pause(true);

        SaveData.current.coins += TestPlayer.Instance.coins;
        SaveLoad.Save();

        StartCoroutine(Ninja.Util.Func.WaitAndRunActionInRealTime(
            1f, 
            () => {
                GameMaster.ResetGame();
                TogglePanel(StartPanel);
            }));
    }
}
