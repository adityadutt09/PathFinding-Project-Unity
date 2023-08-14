using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapElem<Node>
{
    public bool isWalkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    public Node parent;
    public int gCost;
    public int hCost;
    int heapIndex;

    public Node(bool _iswalkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        isWalkable = _iswalkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;

    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node n)
    {
        int compare = fCost.CompareTo(n.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(n.hCost);
        }
        return -compare;
    }

}
