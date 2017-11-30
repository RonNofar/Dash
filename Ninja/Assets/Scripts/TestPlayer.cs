using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

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

    private new Transform transform;
    private Vector3[] positionQue;
    private int queLength = 0;
    private bool isMoving = false;
    private bool isStartMoving = false;
    private bool isRamp = false;

    private GameObject[] targetPool;

    private Coroutine currCoroutine;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Start () {
        SetMaxQueLength(maxQueLength);
        //Debug.Log(targetPrefab.transform.rotation);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClick();
        }
        if (!isMoving && isStartMoving)
        {
            isStartMoving = false;
            currCoroutine = StartCoroutine(StartLerpSequence(positionQue, lerpLength));
        }
    }

    void onClick()
    {
        //Debug.Log("In onClick");
        if (queLength < maxQueLength)
        {
            //Debug.Log("queLength < maxLength");
            for (int i = 0; i < maxQueLength; ++i)
            {
                //Debug.Log("l"+i+" queLEngth: "+queLength);

                if (i == queLength)
                {
                    //Debug.Log("i: "+i+" | queLength: "+queLength);
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, rayRange))
                    {
                        if (hit.transform.tag == "Ground")
                        {
                            Debug.Log(hit.point);
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

            totalTime = distance / lerpVelocity;
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
                    positionQue = new Vector3[maxQueLength];
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
            positionQue = new Vector3[maxQueLength];
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
        positionQue = new Vector3[length];
        targetPool = new GameObject[length];
        // Oh no, don't lose references (transfer them here), I'll finish this later >.<
    }

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


