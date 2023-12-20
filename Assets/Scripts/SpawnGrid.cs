using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class SpawnGrid : MonoBehaviour
    {
        public static SpawnGrid instance;

        public Material[] materials;

        public GameObject hexagonPrefab;
        public float YhexagonSize = 1.0f;
        public float XhexagonSize = 1.0f;
        public Vector2Int[,] coordinates;

        public List<GameObject> hexagons;

        public int totalPathCost;

        [Header("height and width")]
        public int height;
        public int width;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            coordinates = new Vector2Int[(height * 10) + 1, width];
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
            // Offsets
            float xOffset = XhexagonSize;
            float yOffset = YhexagonSize;

            for (int row = 0; row <= height; row++)
            {
                for (int column = 0; column <= width; column++)
                {
                    float xPos = column * xOffset;

                    // Offset every other row
                    if (row % 2 != 0)
                    {
                        xPos += xOffset / 2f;
                    }

                    float zPos = row * yOffset;

                    Vector3 hexagonPosition = new Vector3(xPos, 0, zPos);

                    coordinates[column, row] = new Vector2Int(column, row);

                    var temp = Instantiate(hexagonPrefab, hexagonPosition, Quaternion.identity);

                    temp.GetComponent<Node>().position = coordinates[column, row];
                    temp.name = column + "," + row;

                    var type = Random.Range(0, materials.Length);
                    temp.GetComponent<Node>().SetType(type);

                    hexagons.Add(temp);
                }
            }
        }
    }

}
