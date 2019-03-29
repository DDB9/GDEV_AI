using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform targetFood;
    public Transform targetHome;
    public float speed;
    public bool carrying;
    public GameObject foodLoot;

    Vector3[] path;
    int targetIndex;
    PathRequestManager prm;

    private void Awake() {
        prm = FindObjectOfType<PathRequestManager>();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful) {
        if (pathSuccesful) {
            targetIndex = 0;
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        if (prm.isProcessingPath){
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
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                transform.LookAt(currentWaypoint);
            }
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
