using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Ant : Unit {

    TileGrid grid;

    [Task]
    void RequestFoodPath() {
        if (!carrying) {
            PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
            Task.current.Succeed();
        }
        else if (carrying) {
            Task.current.Fail();
        }
    }
    [Task]
    void RequestAntPath() {
        if (!carrying) {
            // PathRequestManager.RequestPath(transform.position, targetAnt.position, OnPathFound);
            Task.current.Succeed();
        }
        else if (carrying) {
            Task.current.Fail();
        }
    }
    [Task]
    void RequestPathHome() {
        PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        Task.current.Succeed();
    }
    [Task]
    void Flee() {
        PathRequestManager.RequestPath(transform.position, Random.insideUnitCircle * 3, OnPathFound);
        Task.current.Succeed();
    }
    [Task]
    void Hide() {   // Checks if there are any hiding spaces in the vicinity. If so, it hides. Else, the task fails.
        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one, Quaternion.identity);
        List<Transform> hidingSpaces = new List<Transform>();
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

    private void Start() {
        targetFood = GameObject.FindGameObjectWithTag("Food").transform;

        grid = GameObject.Find("Grid").GetComponent<TileGrid>();

        if (!carrying) {
            PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
        }
        else {
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
    }

    private void Update() {
        if (!carrying) {
             PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
        }
        else {
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Food Aquired!");

        if (other.CompareTag("Food")) { //If the ant has aquired food...
            carrying = true;
            foodLoot.SetActive(true);
            grid.CreateGrid();
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
        
        if (other.CompareTag("Home") && carrying) {  // If the ant has arrived safely at home and has some food...
            carrying = false;
            foodLoot.SetActive(false);
            grid.CreateGrid();
            PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
            // Deduct some points.
        }
    }

}
