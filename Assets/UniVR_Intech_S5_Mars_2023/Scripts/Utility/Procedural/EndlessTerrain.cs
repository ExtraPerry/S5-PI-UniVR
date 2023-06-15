using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class EndlessTerrain : MonoBehaviour
{
    [HideInInspector]
    public static float maxViewDist = 450;
    public LODInfo[] detailLevels = new LODInfo[7]
    {
        new LODInfo(0, 0),
        new LODInfo(1, 120),
        new LODInfo(2, 175),
        new LODInfo(3, 225),
        new LODInfo(4, 325),
        new LODInfo(5, 400),
        new LODInfo(6, 450)
    };
    public PlayerRig playerRig;
    public Transform mockPlayer;
    public bool useMockPlayer = false;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    private static MapGenerator mapGenerator;
    private int chunkSize;
    private int chunksVisibleInViewDist;

    private Dictionary<Vector2, TerrainChunk> terrainChunkDisctionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGenerator = gameObject.GetComponent<MapGenerator>();

        maxViewDist = detailLevels[detailLevels.Length - 1].visibleDistThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDist = Mathf.RoundToInt(maxViewDist / chunkSize);

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (useMockPlayer && mockPlayer is not null)
        {
            viewerPosition = new Vector2(mockPlayer.position.x, mockPlayer.position.z);
        }
        else
        {
            viewerPosition = new Vector2(playerRig.xrRig.headset.position.x, playerRig.xrRig.headset.position.z);
        }
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDist; yOffset <= chunksVisibleInViewDist; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDist; xOffset <= chunksVisibleInViewDist; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDisctionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDisctionary[viewedChunkCoord].UpdateTerrainChunk();
                    if (terrainChunkDisctionary[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDisctionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDisctionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk
    {
        private GameObject meshObject;
        private Vector2 position;
        private Bounds bounds;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        private LODInfo[] detailLevels;
        private LODMesh[] lodMeshes;

        private MapData mapData;
        private bool mapDataReceived;
        private int previousLODIndex = -1;

        private bool isGenerated = false;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;

            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshObject.isStatic = true;

            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;

            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < lodMeshes.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].detail);
                mapGenerator.RequestMapData(OnMapDataReceived);
            }
        }

        private void OnMapDataReceived(MapData mapData)
        {
            this.mapData = mapData;
            mapDataReceived = true;
            mapGenerator.RequestMeshData(mapData, 0, OnMeshDataReceived);
        }

        private void OnMeshDataReceived(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }

        public void UpdateTerrainChunk()
        {
            if (mapDataReceived)
            {
                float viewerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewerDistFromNearestEdge <= maxViewDist;

                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewerDistFromNearestEdge > detailLevels[i].visibleDistThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // This is a fix to an issue were when generating terrain for the first time if the game lags it defaults to the LOD => 0 :/
                    if (lodIndex != previousLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                }

                if (!isGenerated)
                {
                    if (lodMeshes[lodMeshes.Length - 1].hasMesh)
                    {
                        meshFilter.mesh = lodMeshes[lodMeshes.Length - 1].mesh;
                        previousLODIndex = lodMeshes.Length - 1;
                        isGenerated = true;
                    }
                }

                SetVisible(visible);
            }
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }

    private class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        public int lod;

        public LODMesh(int lod)
        {
            this.lod = lod;
        }

        private void OnMeshDataRecieved(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int detail;
        public float visibleDistThreshold;

        public LODInfo(int detail, float visibleDistThreshold)
        {
            this.detail = detail;
            this.visibleDistThreshold = visibleDistThreshold;
        }
    }
}
