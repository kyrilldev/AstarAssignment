using Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathing
{
    public enum Type
    {
        Desert = 0,
        Forest,
        Grass,
        Mountain,
        Water
    }
    public class Node : MonoBehaviour, IAStarNode
    {
        public Type Type;
        public Vector2Int position;
        public int cost;

        public Node goal;

        [SerializeField] private int neighbourCount;

        public List<IAStarNode> neighbours;

        IEnumerable<IAStarNode> IAStarNode.Neighbours => neighbours;

        public float Cost()
        {
            return cost;
        }

        public IAStarNode[] Neighbours()
        {
            int CurrentX = position.x;
            int CurrentY = position.y;

            int[] indeces = GetArrayIndeces(CurrentX, CurrentY);

            IAStarNode[] nodes = new IAStarNode[indeces.Length];
            for (int i = 0; i < indeces.Length; i++)
            {
                //print(indeces[i] + "current x pos: " + CurrentX + "current y pos: " + CurrentY);
                nodes[i] = SpawnGrid.instance.hexagons[indeces[i]].GetComponent<Node>();
            }
            return nodes;
        }

        public int[] GetArrayIndeces(int X, int Y)
        {
            List<int> neighborIndices = new();

            // Define the six possible directions for neighbors
            int[,] directions = {
            { 1, 0 }/*right*/, { 0, -1 }/*down*/, { -1, -1 }/*right and down*/,
            { -1, 0 }/*left*/, { -1, 1 }/*left and up*/, { 0, 1 }/*up*/
            };

            // Offset every other row
            int offset = Y % 2 == 0 ? 0 : 1;

            // Calculate indices of neighbors
            for (int i = 0; i < 6; i++)
            {
                int neighborColumn = X + directions[i, 0];
                int neighborRow = Y + directions[i, 1] + offset;

                // Check if the neighbor is within the grid bounds
                if (neighborColumn >= 0 && neighborColumn < SpawnGrid.instance.width && neighborRow >= 0 && neighborRow < SpawnGrid.instance.height)
                {
                    // Calculate the index in the hexagons list
                    int neighborIndex = neighborRow * (SpawnGrid.instance.width + 1) + neighborColumn;

                    neighborIndices.Add(neighborIndex);
                }
                //print(i + "th: " + neighborIndices[i]);
            }

            //print(neighborIndices.Count);

            return neighborIndices.ToArray();
        }

        public int GetIndex(int CurrentX, int CurrentY)
        {
            return (CurrentY * 50) + CurrentX;
        }

        public Vector2Int Position()
        {
            return position;
        }

        public float CostTo(IAStarNode neighbour)
        {
            float costToNeighbour = 0;

            if (neighbour != null)
            {
                costToNeighbour = neighbour.Cost();
            }

            return costToNeighbour;
        }

        public float EstimatedCostTo(IAStarNode goal)
        {
            int currentX = position.x;
            int currentY = position.y;

            int goalX = goal.Position().x;
            int goalY = goal.Position().x;

            float distance = Mathf.Sqrt(Mathf.Pow(goalX - currentX, 2) + Mathf.Pow(goalY - currentY, 2));

            float estimatedCost = distance * goal.Cost();

            return estimatedCost;
        }

        private void Start()
        {
            neighbours = Neighbours().ToList();

            print("neighbour count is: " + neighbours.Count);

            neighbourCount = neighbours.Count;
        }

        public void SetType(int type)
        {
            GetComponent<MeshRenderer>().material = SpawnGrid.instance.materials[type];
            switch (type)
            {
                case 0:
                    Type = Type.Desert;
                    cost = 5;
                    break;
                case 1:
                    Type = Type.Forest;
                    cost = 3;
                    break;
                case 2:
                    Type = Type.Grass;
                    cost = 1;
                    break;
                case 3:
                    Type = Type.Mountain;
                    cost = 10;
                    break;
                case 4:
                    Type = Type.Water;
                    cost = 1000;
                    break;
            }
        }

        GameObject IAStarNode.GameObject()
        {
            return this.gameObject;
        }
    }

}
