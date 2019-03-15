using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Unit {

    private void Start() {
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
            other.transform.parent = this.transform;
            hasFood = true;
            PathRequestManager.RequestPath(transform.position, targetHome.position, OnPathFound);
        }
        if (other.CompareTag("Home") && hasFood) {
            Destroy(this.transform.GetChild(0));
            // Deduct some points.
        }
    }

}
