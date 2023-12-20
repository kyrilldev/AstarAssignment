using Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public bool testing;

    public List<IAStarNode> nodes2;

    IEnumerable<IAStarNode> IAStarNode.Neighbours => Neighbours();

    public float Cost()
    {
        return cost;
    }

    public IAStarNode[] Neighbours()
    {
        int CurrentX = position.x;
        int CurrentY = position.y;

        int[] indeces = GetArrayIndeces(CurrentX, CurrentY);

        IAStarNode[] nodes = new IAStarNode[6];
        for (int i = 0; i < nodes.Length; i++)
        {
            print(indeces[i] + "current x pos: " + CurrentX + "current y pos: " + CurrentY);
            if (indeces[i] >= 0)
            {
                nodes[i] = SpawnGrid.instance.hexagons[indeces[i]].GetComponent<Node>();
            }
            else
            {
                nodes[i] = null;
            }
        }
        return nodes;
    }

    public int[] GetArrayIndeces(int X, int Y)
    {
        int[] array = new int[6];
        array[0] = (X + 1) + Y;
        array[1] = (X - 1) + Y;
        array[2] = (X - 2) + Y;
        array[3] = (X - 1) + (Y + 51);
        array[4] = (X + 1) + (Y + 51);
        array[5] = (X + 2) + Y;

        return array;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && testing)
        {
            nodes2 = Neighbours().ToList();
        }
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
                cost = 0;
                break;
        }
    }

}
