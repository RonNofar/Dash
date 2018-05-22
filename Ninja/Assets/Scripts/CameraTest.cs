using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

    public GameObject anObject;
    public Collider anObjCollider;
    private Camera cam;
    private Plane[] planes;

    public float movementSpeed = 0.1f;
    public float waitTime = 1f;

    private new Transform transform;
    private float startTime;

    [Header("DEBUG")]
    [SerializeField]
    private bool __DEBUG__ = false;

    void Start()
    {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        anObjCollider = anObject.GetComponent<Collider>();
        //Debug.Log(anObjCollider.bounds);

        transform = GetComponent<Transform>();
        startTime = Time.time;
    }
    void Update()
    {
        if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
        {
            if (__DEBUG__) Debug.Log(anObject.name + " has been detected!");

        }
        else
        {
            if (__DEBUG__) Debug.Log("Nothing has been detected");

        }

        if (Time.time > startTime + waitTime)
            transform.Translate(Vector3.forward * movementSpeed, Space.World);
    }
}

