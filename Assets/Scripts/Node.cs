using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, IAStarNode
{
    public IEnumerable<IAStarNode> Neighbours => throw new System.NotImplementedException();

    public float CostTo(IAStarNode neighbour)
    {
        throw new System.NotImplementedException();
    }

    public float EstimatedCostTo(IAStarNode goal)
    {
        throw new System.NotImplementedException();
    }
}
