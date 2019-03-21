using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Unit : MonoBehaviour
{
    public Transform targetFood;
    public Transform targetHome;
    public float speed;
    public bool carrying;
    public GameObject foodLoot;

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
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            transform.LookAt(currentWaypoint);
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

    [Task]
    void RequestFoodPath() {    // Requests a path to the objective.
        if (!carrying) {
            PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
            Task.current.Succeed();
        }
        else if (carrying) {
            Task.current.Fail();
        }
    }
    [Task]
    void RequestAntPath() { // Requests a path to a dead ant.
        if (!carrying) {
            // PathRequestManager.RequestPath(transform.position, targetAnt.position, OnPathFound); (Will be dead ant position).
            Task.current.Succeed();
        }
        else if (carrying) {
            Task.current.Fail();
        }
    }
    [Task]
    void RequestPathHome() {    // Requests a path to the Homebase.
        PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        Task.current.Succeed();
    }
    [Task]
    void Flee() {   // Unit Flees to a random direction.
        PathRequestManager.RequestPath(transform.position, Random.insideUnitCircle * 3, OnPathFound);
        Task.current.Succeed();
    }
    [Task]
    void Hide() {   // Checks if there are any hiding spaces in the vicinity. If so, it hides. Else, the task fails.
        List<Transform> hidingSpaces = new List<Transform>();

        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one, Quaternion.identity);
        foreach (Collider collider in cols) {
            if (collider.CompareTag("Obstacle")) {
                hidingSpaces.Add(collider.transform);
            }
        }

        if (hidingSpaces.Count > 0) {
            PathRequestManager.RequestPath(transform.position, hidingSpaces[Random.Range(0, hidingSpaces.Count)].position, OnPathFound);
            Task.current.Succeed();
        }
        else if (hidingSpaces.Count <= 0) {
            Task.current.Fail();
        }
    }

}
