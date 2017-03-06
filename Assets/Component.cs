using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Component : MonoBehaviour {
    public float EMF = 0;

    public float voltageIn;
    public float voltageOut;
    public float resistance;
    public float current;
    public bool calculated = false;
    public List<GameObject> connected = new List<GameObject>();


    public void findConnected() {
        GameObject[] stuffs = GameObject.FindGameObjectsWithTag("Component");
        foreach (GameObject g in stuffs) {
            if (!g.Equals(this.gameObject) && g.GetComponent<Collider>() != null && g.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds)) {
                connected.Add(g);
            }
        }
    }

    public void calculateVoltageIn() {
        float sum = 0;
        foreach (GameObject g in connected) {
            Component c = g.GetComponent<Component>();
            if (c != null) {
                if (c.getVoltageOut() >= 0) {
                    sum += 1 / c.getVoltageOut();
                }
            }
        }
        print(this.gameObject.name + " : " + sum);
        if (sum != 0)
            voltageIn = 1 / sum + EMF;
        else
            voltageIn = EMF;

        calculated = true;

        foreach (GameObject g in connected) {
            Component c = g.GetComponent<Component>();
            if (c != null) {
                if (!c.calculated) {
                    c.calculateVoltageIn();
                }
            }
        }
        getVoltageOut();
    }

    public float getVoltageOut() {
        //Haven't calculated current yet so ignore resistance
        voltageOut = voltageIn - .1f;
        return voltageOut;
    }

	// Use this for initialization
	void Start () {
        findConnected();
        if (EMF != 0)
            calculateVoltageIn();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void FixedUpdate() {

    }
}
