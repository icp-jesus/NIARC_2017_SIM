using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour {

    //N*m ->

    private WheelCollider wheel;
    private float maxMotorTorque;
    private float maxRmp;
    private float appliedTorque = 0;
    private string motorName;

    private void Start()
    {
        
    }

    public void Init(float maxRpm, float maxTorque, string name)
    {
        wheel = gameObject.GetComponent<WheelCollider>();
        motorName = name;
        maxMotorTorque = maxTorque;
        wheel.ConfigureVehicleSubsteps(maxRpm, 2, 2);
    } 

    // Update is called once per frame
    void FixedUpdate () {
         wheel.motorTorque = maxMotorTorque * appliedTorque;
    }

    public void ApplyTorque(float forward_neg1_to_pos1, float lateral_neg1_to_pos1)
    {
        appliedTorque = Mathf.Clamp(forward_neg1_to_pos1, -1.0F, 1.0F);
        appliedTorque += lateral_neg1_to_pos1;
    }
}
