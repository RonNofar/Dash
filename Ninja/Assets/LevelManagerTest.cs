using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerTest : MonoBehaviour {

    [SerializeField]
    private GameObject[] tiles;

    private int totalTiles;
    private Transform[] tileTransforms;

    private void Awake()
    {
        totalTiles = tiles.Length;
        tileTransforms = new Transform[totalTiles];
        for (int i = 0; i < totalTiles; ++i)
        {
            tileTransforms[i] = tiles[i].GetComponent<Transform>();
        }
    }

    private void FixedUpdate()
    {

    }
}
