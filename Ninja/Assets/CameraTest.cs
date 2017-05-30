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

    void Start()
    {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        anObjCollider = anObject.GetComponent<Collider>();
        Debug.Log(anObjCollider.bounds);

        transform = GetComponent<Transform>();
        startTime = Time.time;
    }
    void Update()
    {
        if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
            Debug.Log(anObject.name + " has been detected!");
        else
            Debug.Log("Nothing has been detected");

        if (Time.time > startTime + waitTime)
            transform.Translate(Vector3.forward * movementSpeed, Space.World);
    }
}

