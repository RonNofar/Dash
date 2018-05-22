using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour {

    static protected TestPlayer _instance;
    static public TestPlayer Instance { get { return _instance; } }

    [SerializeField]
    private float rayRange = 100f;
    [SerializeField]
    private float lerpLength = 1f;
    [SerializeField]
    private float lerpVelocity = 5f;
    [SerializeField]
    private int maxQueLength = 2; // NOTE: if changed mid-game it WILL cause an index out of bounds error, must write a way to handle.
    [SerializeField]
    private GameObject targetPrefab;
    [SerializeField]
    private Vector3 targetLocalSpawnPosition = new Vector3(0, 1f, 0);
    [SerializeField]
    private float rampBoost = 0.1f;

    [Header("PickUps")]
    [SerializeField] TimeManager timeManager;
    [SerializeField] Text coinText;

    private new Transform transform;
    private Vector3[] positionQue;
    private int queLength = 0;
    private bool isMoving = false;
    private bool isStartMoving = false;
    private bool isRamp = false;

    private GameObject[] targetPool;

    private Coroutine currCoroutine;

    [HideInInspector]
    public int coins = 0;

    public Dictionary<Upgrade.Type, int> upgrades = new Dictionary<Upgrade.Type, int>();

    private float velocityUpgrade;    // velocity after calculations;
    private int maxQueLengthUgrade; // the max que length after initializing upgrades


    #region Unity Functions
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Player is already in play. Deleting new!", gameObject);
            Destroy(gameObject);
        }
        else
        { _instance = this; }


        transform = GetComponent<Transform>();
    }

    void Start () {
        InitializeUpgrades();

        SetMaxQueLength(
            Upgrade.CalculateQue(
                maxQueLength, 
                upgrades[Upgrade.Type.QUE]));

        //Debug.Log(targetPrefab.transform.rotation);

        coinText.text = "" + coins;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameMaster.Instance.isPaused)
                onClick();
        }
        if (!isMoving && isStartMoving)
        {
            isStartMoving = false;
            currCoroutine = StartCoroutine(StartLerpSequence(positionQue, lerpLength));
        }
    }
    #endregion

    #region Movement Functions
    void onClick()
    {
        //Debug.Log("In onClick");
        if (queLength < maxQueLengthUgrade)
        {
            //Debug.Log("queLength < maxLength");
            for (int i = 0; i < maxQueLengthUgrade; ++i)
            {
                //Debug.Log("l"+i+" queLEngth: "+queLength);

                if (i == queLength)
                {
                    //Debug.Log("i: "+i+" | queLength: "+queLength);
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, rayRange))
                    {
                        if (hit.transform.tag == "Ground" || hit.transform.tag == "Ramp")
                        {
                            //Debug.Log(hit.point);
                            Vector3 hitXZ = new Vector3(
                                hit.point.x,
                                transform.position.y,
                                hit.point.z
                            );
                            positionQue[i] = hitXZ;
                            if (i == 0) isStartMoving = true;
                            //StartCoroutine(StartLerpSequence(positionQue, lerpLength));
                            if (targetPool[i] == null)
                            {
                                targetPool[i] = Instantiate(
                                    targetPrefab, 
                                    hit.point + targetLocalSpawnPosition, 
                                    targetPrefab.transform.rotation);
                                //targetPool[i].GetComponent<Ninja.target>().totalTime = 
                            } else
                            {
                                targetPool[i].SetActive(true);
                                targetPool[i].transform.position = hit.point + targetLocalSpawnPosition;
                            }

                            ++queLength;
                        }
                    }
                    break;
                }
            }
        }
    }



    IEnumerator StartLerpSequence(Vector3[] toLerpArray, float? totalTime)
    {
        isMoving = true;
        float startTime = Time.time;
        float timeRatio = 0f;
        Vector3 orgPos = transform.position;

        int i = 0;
        
        while (timeRatio < 1)
        {
            Vector3 toLerp = toLerpArray[i];
            //Debug.Log("ToLerp: " + toLerp);

            float distance = Vector3.Distance(orgPos, toLerp);

            totalTime = distance / velocityUpgrade;
            timeRatio = (Time.time - startTime) / (float)totalTime;
            if (timeRatio > 1) timeRatio = 1;

            if (isRamp)
            {
                //transform.Translate(Vector3.up * rampBoost + -Physics.gravity);
                toLerpArray[i] = new Vector3(toLerp.x, toLerp.y + rampBoost, toLerp.z);
                toLerp = toLerpArray[i];
                Debug.Log("In sequence and RAMP");
            }
            transform.position = Vector3.Lerp(orgPos, toLerp, timeRatio);
            

            if (timeRatio == 1)
            { // Clean up here
                targetPool[i].SetActive(false);
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (i+1 < queLength)
                {
                    ++i;
                    startTime = Time.time;
                    timeRatio = 0;
                    orgPos = transform.position;
                }
                else
                {
                    positionQue = new Vector3[maxQueLengthUgrade];
                    queLength = 0;
                    isMoving = false;
                    Debug.Log("Broke");
                    break;
                }
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Obstacle")
        {
            StopCoroutine(currCoroutine);
            positionQue = new Vector3[maxQueLengthUgrade];
            queLength = 0;
            isMoving = false;
            for (int i = 0; i < targetPool.Length; ++i)
            {
                if (targetPool[i] != null)
                    targetPool[i].SetActive(false);
            }

            Debug.Log("Collision");
        }
        else if (col.transform.tag == "Ramp")
        {
            Debug.Log("RAMP");
            //transform.Translate(Vector3.up * rampBoost);
            //Vector3 pos = transform.position;
            //transform.position = new Vector3(pos.x, pos.y + rampBoost, pos.z);
            isRamp = true;
        }
    }/*
    private void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Ramp")
        {
            Debug.Log("RAMP");
            transform.Translate(Vector3.up * rampBoost);
        }
    }*/
    private void OnCollisionExit(Collision col)
    {
        if (col.transform.tag == "Ramp")
        {
            Debug.Log("RAMP UOT");
            isRamp = false;
        }
    }

    void SetMaxQueLength(int length)
    { // This method is used to handle a change in max que length while running
        maxQueLengthUgrade = length;
        positionQue = new Vector3[length];
        targetPool = new GameObject[length];
        // Oh no, don't lose references (transfer them here), I'll finish this later >.<
    }
    #endregion

    #region Other Functions
    public void OnPickUp(PickUp.Type type, float amount)
    {
        switch(type)
        {
            case PickUp.Type.NULL:
                Debug.Log("ERROR: PickUpType is NULL");
                break;
            case PickUp.Type.HEALTH:
                timeManager.AddTime(
                    Upgrade.CalculateHealth(
                        amount, 
                        upgrades[Upgrade.Type.HEALTH]));
                break;
            case PickUp.Type.COIN:
                coins += Upgrade.CalculateCoin(
                    (int)amount,
                    upgrades[Upgrade.Type.COIN]);
                //coinText.text = "" + coins;
                break;
        }
    }

    public void InitializeUpgrades()
    {
        SaveData savedData = SaveLoad.Load();
        /*SaveData.current = new SaveData();
        SaveLoad.savedData = SaveData.current;*/
        AddOrChangeDictionaryValueByKey(Upgrade.Type.VELOCITY, savedData);
        AddOrChangeDictionaryValueByKey(Upgrade.Type.HEALTH  , savedData);
        AddOrChangeDictionaryValueByKey(Upgrade.Type.COIN    , savedData);
        AddOrChangeDictionaryValueByKey(Upgrade.Type.TIME    , savedData);
        AddOrChangeDictionaryValueByKey(Upgrade.Type.QUE     , savedData);

        velocityUpgrade =
            Upgrade.CalculateVelocity(
                    lerpVelocity,
                    upgrades[Upgrade.Type.VELOCITY]);

        timeManager.AddTime(
            Upgrade.CalculateTime(
                upgrades[Upgrade.Type.TIME]));

        SetMaxQueLength(
            Upgrade.CalculateQue(
                maxQueLength,
                upgrades[Upgrade.Type.QUE]));

        /*upgrades.Add(
            Upgrade.Type.VELOCITY, 
            savedData.upgrades[Upgrade.Type.VELOCITY]);
        upgrades.Add(
            Upgrade.Type.HEALTH,
            savedData.upgrades[Upgrade.Type.HEALTH]);
        upgrades.Add(
            Upgrade.Type.COIN,
            savedData.upgrades[Upgrade.Type.COIN]);
        upgrades.Add(
            Upgrade.Type.TIME,
            savedData.upgrades[Upgrade.Type.TIME]);*/
    }

    // ADD ratio to this function as input? or elsewhere...
    void AddOrChangeDictionaryValueByKey(Upgrade.Type key, SaveData savedData = null)
    { // adds key / value if does not exist, updates value otherwise 
        if (savedData == null)
            savedData = SaveLoad.Load();

        if (upgrades.ContainsKey(key))
            upgrades[key] = savedData.upgrades[key];
        else
            upgrades.Add(key, savedData.upgrades[key]);
    }
    #endregion
}

/*
 * if mouseclick and que is not full -> add location to que
 *      if new location is 0th index -> start SingleLerp
 *      
 * singlelerp
 *      lerps over time (relative speed)
 *      at end of lerp, pop out of que, readjust que, and lessen quelength accordingly
 *      if quelength > queindex+1 (or if 0th index is not null)
 *          run singlelerp again recursively 
 *  
 *  On collision, stop current coroutine, reset all that needs to be reset
 */


