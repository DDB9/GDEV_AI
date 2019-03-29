using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Ant : Unit {

    [Task]
    void RequestFoodPath() {    // Requests a path to the objective.
        targetFood = GameObject.FindGameObjectWithTag("Food").transform;
        PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
        Task.current.Succeed();

    }
    
    [Task]
    void RequestPathHome() {    // Requests a path to the Homebase.
        GameObject[] homes = GameObject.FindGameObjectsWithTag("Home");

        PathRequestManager.RequestPath(transform.position, homes[Random.Range(0, homes.Length)].transform.position, OnPathFound);
        Task.current.Succeed();
    }

    [Task]
    bool isCarrying() {
        if (carrying) {
            return true;
        }
        else {
            return false;
        }
    }

    [Task]
    bool inRangeOfFoot() {
        Vector3 halfExtends = new Vector3(1f, 1f, 0.5f);
        Collider[] foot = Physics.OverlapBox(this.transform.position, halfExtends, Quaternion.identity);

        foreach (Collider col in foot)
        {
            if (col.CompareTag("Foot")) {
                Task.current.Succeed();
                return true;
            }
        }
        Task.current.Fail();
        return false;
    }
   
    [Task]
    void Flee() {   // Unit Flees to a random direction.
        PathRequestManager.RequestPath(transform.position, Random.insideUnitCircle * 5, OnPathFound);
        Task.current.Succeed();
    }

    [Task]
    void Hide() {   // Checks if there are any hiding spaces in the vicinity. If so, it hides. Else, the task fails.
        List<Transform> hidingSpaces = new List<Transform>();

        Vector3 halfExtends = new Vector3(3f, 1f, 3f);
        Collider[] cols = Physics.OverlapBox(transform.position, halfExtends, Quaternion.identity);
        foreach (Collider collider in cols) {
            if (collider.CompareTag("Obstacle")) {
                hidingSpaces.Add(collider.transform);
            }
        }

        if (hidingSpaces.Count > 0) {
            TileGrid grid = FindObjectOfType<TileGrid>();
            grid.isFleeing = true;
            PathRequestManager.RequestPath(transform.position, hidingSpaces[Random.Range(0, hidingSpaces.Count)].position, OnPathFound);
            grid.isFleeing = false;
            Task.current.Succeed();
        }
        else if (hidingSpaces.Count <= 0) {
            Task.current.Fail();
        }
    }
}
