using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Unit {

    TileGrid grid;

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
