using Pathing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public Material[] materials;

    public GameObject hexagonPrefab;
    public float YhexagonSize = 1.0f;
    public float XhexagonSize = 1.0f;

    public List<GameObject> hexagons;

    [Header("height and width")]
    public int gridRadius = 5;
    public int width;

    private void Start()
    {
        SpawnHexGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            RestartGeneration();
        }
    }

    private void RestartGeneration()
    {
        for (int i = 0; i < hexagons.Count; i++)
        {
            Destroy(hexagons[i]);
        }
        hexagons.Clear();
        SpawnHexGrid();
    }

    public void SpawnHexGrid()
    {
        //offsets
        float xOffset = XhexagonSize * 0.5f;
        float yOffset = 1.5f * YhexagonSize;

        for (int row = -gridRadius; row <= gridRadius; row++)//row == -5, zolang row kleiner of equal is aan 5, row++
        {
            int end = width;

            for (int i = 0; i <= end; i++)//int = 0, zolang 0 kleiner of equal is aan 5
            {
                float xPos = i * xOffset;//
                float zPos = row * yOffset;

                // Offset every other row
                if (i % 2 != 0)
                {
                    zPos += yOffset / 2;
                }

                Vector3 hexagonPosition = new(xPos, 0, zPos);
                var temp = Instantiate(hexagonPrefab, hexagonPosition, Quaternion.identity);
                temp.name += row + i;
                temp.AddComponent<MeshCollider>();
                temp.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
                hexagons.Add(temp);
            }
        }
    }
}
