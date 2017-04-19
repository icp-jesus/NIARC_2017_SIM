using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentialDrive : MonoBehaviour
{
    private int cnt = 0;
    public bool manualControl = false;
    public GameObject wheelLeft;
    public GameObject wheelRight;
    public GameObject pathFollowing;
    public float MaxForwardVelocity;
    public float MaxTorque;

    public float Mass;
    public float Radius;
    public float WheelDampintRate;
    public float SuspensionDistance;
    public float ForceAppPointDistance;

    public float SuspensionSpring;
    public float SuspensionDamper;
    public float SuspensionTargetPosition;

    public float FricForwardExtremumSlip;
    public float FricForwardExtremumValue;
    public float FricForwardAsymptoteSlip;
    public float FricForwardAsymptoteValue;
    public float FricForwardStiffness;

    public float FricSideExtremumSlip;
    public float FricSideExtremumValue;
    public float FricSideAsymptoteSlip;
    public float FricSideAsymptoteValue;
    public float FricSideStiffness;


    private Motor left;
    private Motor right;

    // Use this for initialization
    void Start()
    {
        left = wheelLeft.GetComponent<Motor>();
        right = wheelRight.GetComponent<Motor>();
        left.Init();
        right.Init();
    }

    public void ApplyTorque(float acel, float turn)
    {
        left.ApplyTorque(acel, turn / 2.0f);
        right.ApplyTorque(acel, turn / -2.0f);
    }
    public float MaxRPM
    {
        //60rpm = 2PI rad/sec
        //1rpm  = (2PI/60) rad/sec
        //maxRadPerSec = maxRpm * (PI/30)
        //maxVel = wheelRadius * maxRpm * PI/30
        //maxRpm = maxVel / (wheelRadius * PI/30)
        get
        {
            return MaxForwardVelocity / (Radius * Mathf.PI / 30.0f);
        }
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float acel = 0;
        float turn = 0;
        //float acel1 = 0;
        //float turn1 = 0;
        
        if (manualControl)
        {
            acel = Input.GetAxis("Vertical");
            turn = Input.GetAxis("Horizontal");
        }
        else
        {
            acel = 1 - pathFollowing.GetComponent<PathFollowing>().SpeedCommand;
            turn = pathFollowing.GetComponent<PathFollowing>().TurnCommand;
        }
        
        if(cnt%5 == 0)
        {
            //Debug.Log("PointCount: " + pathFollowing.GetComponent<PathFollowing>().breadCrumbCount);
            //Debug.Log("Acel: " + acel);
            //Debug.Log("Turn: " + turn);
            cnt = 1;
        }
        cnt++;
        ApplyTorque(acel, turn);
    }
}
