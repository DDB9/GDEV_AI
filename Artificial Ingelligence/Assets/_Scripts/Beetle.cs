using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Beetle : Unit {

    public bool stompedOn = false;
    public float health = 2;

    void Update() {
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

    [Task]
    void RequestFoodPath() {    // Requests a path to the objective.
        if (stompedOn) {
            speed = 20;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            speed = 10;
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        targetFood = GameObject.FindGameObjectWithTag("Food").transform;
        PathRequestManager.RequestPath(transform.position, targetFood.position, OnPathFound);
        Task.current.Succeed();

    }
    
    [Task]
    void RequestPathHome() {    // Requests a path to the Homebase.
        if (stompedOn) {
            speed = 20;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            speed = 10;
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
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
    void Enrage() {
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        speed = 0;
        if (stompedOn){
            speed = 20;
            Task.current.Succeed();
        }
        Task.current.Fail();
    }

}
