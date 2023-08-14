using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    PathManager manager;

    private void Awake()
    {
        manager = GetComponent<PathManager>();
        grid = GetComponent<Grid>();
    }
   

    public void StartNextProcess(Vector3 start, Vector3 goal)
    {
        StartCoroutine(FindPath(start, goal));

    }
    IEnumerator FindPath(Vector3 start, Vector3 goal)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.getNodeFromWorldPos(start);
        Node goalNode = grid.getNodeFromWorldPos(goal);

        if (startNode.isWalkable && goalNode.isWalkable)
        {

            Heap<Node> open = new Heap<Node>(grid.MaxGridSize);
            HashSet<Node> closed = new HashSet<Node>();

            open.Add(startNode);

            while (open.Count > 0)
            {
                Node currentNode = open.RemoveFirstElem();
                closed.Add(currentNode);

                //Early Exit
                if (currentNode == goalNode)
                {
                    sw.Stop();
                    print("Path found in: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node n in grid.getNeighbours(currentNode))
                {
                    if (!n.isWalkable || closed.Contains(n))
                        continue;

                    int newCost = currentNode.gCost + GetDistance(currentNode, n);

                    if (newCost < n.gCost || !open.Contains(n))
                    {
                        n.gCost = newCost;
                        n.hCost = GetDistance(n, goalNode);
                        n.parent = currentNode;

                        if (!open.Contains(n))
                            open.Add(n);
                    }

                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = retracePath(startNode, goalNode);
        }
        manager.FinishProcess(waypoints, pathSuccess);
    }

    Vector3[] retracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;

        while(current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDir = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 newDir = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(newDir != oldDir)
            {
                waypoints.Add(path[i].worldPos);
            }
            oldDir = newDir;
        }

        return waypoints.ToArray(); 
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }



}
