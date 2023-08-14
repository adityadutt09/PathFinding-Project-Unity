using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathManager : MonoBehaviour
{
    Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    
    PathRequest currentRequest;
    
    static PathManager instance;

    PathFinding pathfinding;
    bool isProcessingPath;


    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }
    public static void RequestPath(Vector3 startPath, Vector3 endPath, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(startPath, endPath, callback);
        instance.requestQueue.Enqueue(newRequest);

        instance.TryNextProcess();

    }

    void TryNextProcess()
    {
        if (!isProcessingPath)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartNextProcess(currentRequest.startPath, currentRequest.endPath);
        }
    }

    public void FinishProcess(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessingPath = false;
        TryNextProcess();
    }

    struct PathRequest
    {
        public Vector3 startPath;
        public Vector3 endPath;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _startPath, Vector3 _endPath, Action<Vector3[], bool> _callback)
        {
            startPath = _startPath;
            endPath = _endPath;
            callback = _callback;
        }

    }

}
