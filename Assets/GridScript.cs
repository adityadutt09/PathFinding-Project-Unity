using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        //Keeps track of number of feasible nodes within the gridWorldSize
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (2 * nodeRadius));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (2 * nodeRadius));
        CreateGrid();

    }

    //Builds a grid of Nodes
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * 2 * nodeRadius + nodeRadius) + Vector3.forward * (y * 2 * nodeRadius + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }

    }

    public int MaxGridSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }


    //See comments for testing if function works
    public Node getNodeFromWorldPos(Vector3 worldPos)
    {
        float percentX = Mathf.Clamp01( ( worldPos.x + gridWorldSize.x / 2 ) / gridWorldSize.x );
        float percentY = Mathf.Clamp01( ( worldPos.z + gridWorldSize.y / 2 ) / gridWorldSize.y );

        int x_index = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y_index = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x_index, y_index];

    }

    public List<Node> getNeighbours(Node current)
    {
        List<Node> neighbours = new List<Node>();

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int newX = current.gridX + i;
                int newY = current.gridY + j;

                if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                {
                    neighbours.Add(grid[newX, newY]);
                }
            }
        }
        return neighbours;
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.isWalkable) ? Color.white : Color.black;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeRadius * 1.8f));
            }
        }
    }
}

/*    public Transform player;
 *    
 *    Add the below code to DrawGizmos()
 *    Node playerNode = getNodeFromWorldPos(player.position);
 *    if (playerNode == n)
 *    {
 *      Gizmos.color = Color.green;
      }
 */