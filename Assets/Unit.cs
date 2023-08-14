using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    float speed = 25f;
    Vector3[] path;
    int targetIndex;


    private void Start()
    {
        PathManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {

        Vector3 currentpoint = path[0];

        while (true)
        {
            if(transform.position == currentpoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                    yield break;
                currentpoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentpoint, speed * Time.deltaTime);
            yield return null;

        }
    }
    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);

            }
            
            
        }
    }

}
