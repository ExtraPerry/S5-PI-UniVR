using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicMeshGenerator))]
[RequireComponent(typeof(MapGenerator))]
public class TempProceduralDesmonstrator : MonoBehaviour
{
    public float timeInterval = 0.5f;

    private float time = 0;

    BasicMeshGenerator meshGen;
    MapGenerator textureGen;

    void Start()
    {
        meshGen = gameObject.GetComponent<BasicMeshGenerator>();
        textureGen = gameObject.GetComponent<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= timeInterval)
        {
            int size = Random.Range(16, 32);
            meshGen.xSize = size;
            meshGen.zSize = size;
            textureGen.mapWidth = size;
            textureGen.mapHeight = size;

            meshGen.CreateShape();
            textureGen.GenerateMap();

            time = 0;
        }
    }
}
