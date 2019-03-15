using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform targetFood;
    public Transform targetHome;
    public float speed;
    public bool hasFood;

    Vector3[] path;
    int targetIndex; 

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful) {
        if (pathSuccesful) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            yield return null;  // Wait for one frame.
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            transform.LookAt(Vector3.forward);
        }
    }

    public void OnDrawGizmos() {
        
    }
}
