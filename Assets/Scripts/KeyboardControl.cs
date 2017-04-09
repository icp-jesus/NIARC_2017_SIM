using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour {

    public GameObject wheelLeft;
    public GameObject wheelRight;
    public float maxRpm;
    public float maxTorque;
    private Motor left;
    private Motor right;

    // Use this for initialization
    void Start () {
        left =  wheelLeft.GetComponent<Motor>();
        right = wheelRight.GetComponent<Motor>();
        left.Init(maxRpm, maxTorque, "left");
        right.Init(maxRpm, maxTorque, "right");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        float acel = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        left.ApplyTorque(acel, turn / 5.0f);
        right.ApplyTorque(acel, turn / -5.0f);
    }
}
