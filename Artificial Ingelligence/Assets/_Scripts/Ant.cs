using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Unit {

    TileGrid grid;

    private void Start() {
        grid = GameObject.Find("Grid").GetComponent<TileGrid>();

        if (!hasFood) {
            PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
        }
        else {
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
    }

    private void Update() {
        if (!hasFood) {
            PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
        }
        else {
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Food Aquired!");

        if (other.CompareTag("Food")) {
            hasFood = true;
            foodLoot.SetActive(true);
            grid.CreateGrid();
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
        
        if (other.CompareTag("Home") && hasFood) {
            Destroy(this.transform.GetChild(0));
            hasFood = false;
            Destroy(this.gameObject);
            // Deduct some points.
        }
    }

}
