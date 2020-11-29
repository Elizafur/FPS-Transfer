     
    using UnityEngine;
    using UnityEditor;
     
    // Set the height map of terrain to collider hits.  This allows matching terrain
    // to a mesh.
    public class SetTerrainFromMesh
    {
        [MenuItem("Edit/Match terrain to mesh")]
        static void Go()
        {
            if(Selection.activeTransform == null)
            {
                Debug.LogWarning("Select the terrain");
                return;
            }
     
            Terrain terrain = Selection.activeTransform.GetComponent<Terrain>();
            if(terrain == null)
            {
                Debug.LogWarning("The selection isn't a terrain");
                return;
            }
     
            // Temporarily disable the terrain collider, so our rays don't hit it.
            TerrainCollider terrainCollider = terrain.gameObject.GetComponent<TerrainCollider>();
            bool terrainColliderWasEnabled = terrainCollider.enabled;
            if(terrainCollider != null)
                terrainCollider.enabled = false;
     
            TerrainData terrainData = terrain.terrainData;
            Vector3 worldSize = new Vector3(terrainData.size.z, 0, terrainData.size.x);
            float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
     
            for(int x = 0; x < terrainData.heightmapResolution; ++x)
            {
                for(int y = 0; y < terrainData.heightmapResolution; ++y)
                {
                    // The X axis of terrain is along the Z axis in object space, and the Y axis of
                    // terrain is along the X axis in object space.
                    float worldZ = scale(x, 0, terrainData.heightmapResolution, 0, worldSize.z);
                    float worldX = scale(y, 0, terrainData.heightmapResolution, 0, worldSize.x);
     
                    // Cast a ray downwards from above the terrain.
                    Vector3 worldPos = new Vector3(worldX, 10000, worldZ);
                    worldPos += terrain.transform.position;
     
                    Ray ray = new Ray(worldPos, Vector3.down);
                    RaycastHit hit;
                    if(!Physics.Raycast(ray, out hit))
                    {
                        heights[x,y] = 0;
                        continue;
                    }
     
                    Vector3 hitPoint = hit.point;
                    float hitY = hitPoint.y;
                    hitY -= terrain.transform.position.y;
                    heights[x,y] = hitY / terrainData.size.y;
                }
            }
            terrainData.SetHeights(0, 0, heights);
     
            // Restore the terrain collider.
            if(terrainCollider != null)
                terrainCollider.enabled = terrainColliderWasEnabled;
        }
     
        static public float scale(float x, float l1, float h1, float l2, float h2)
        {
            return (x - l1) * (h2 - l2) / (h1 - l1) + l2;
        }
    }
     
