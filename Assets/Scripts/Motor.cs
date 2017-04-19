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

    //TODO ADD ALL THE THINGS!!!
    public void Init()
    {
        DifferentialDrive initInfo = gameObject.GetComponentInParent<DifferentialDrive>();
        wheel = gameObject.AddComponent<WheelCollider>();
        wheel.mass = initInfo.Mass;
        wheel.radius = initInfo.Radius;
        wheel.wheelDampingRate = initInfo.WheelDampintRate;
        wheel.suspensionDistance = initInfo.SuspensionDistance;
        wheel.forceAppPointDistance = initInfo.ForceAppPointDistance;

        JointSpring newJointSpring = new JointSpring();
        newJointSpring.spring = initInfo.SuspensionSpring;
        newJointSpring.damper = initInfo.SuspensionDamper;
        newJointSpring.targetPosition = initInfo.SuspensionTargetPosition;
        wheel.suspensionSpring = newJointSpring;

        WheelFrictionCurve fricForward = new WheelFrictionCurve();
        fricForward.extremumSlip = initInfo.FricForwardExtremumSlip;
        fricForward.extremumValue = initInfo.FricForwardExtremumValue;
        fricForward.asymptoteSlip = initInfo.FricForwardAsymptoteSlip;
        fricForward.asymptoteValue = initInfo.FricForwardAsymptoteValue;
        fricForward.stiffness = initInfo.FricForwardStiffness;
        wheel.forwardFriction = fricForward;

        WheelFrictionCurve fricSide = new WheelFrictionCurve();
        fricSide.extremumSlip = initInfo.FricSideExtremumSlip;
        fricSide.extremumValue = initInfo.FricSideExtremumValue;
        fricSide.asymptoteSlip = initInfo.FricSideAsymptoteSlip;
        fricSide.asymptoteValue = initInfo.FricSideAsymptoteValue;
        fricSide.stiffness = initInfo.FricForwardStiffness;
        wheel.sidewaysFriction = fricSide;

        maxMotorTorque = initInfo.MaxTorque;
        Debug.Log("MaxRpm: " + initInfo.MaxRPM);
        wheel.ConfigureVehicleSubsteps(initInfo.MaxRPM, 2, 2);
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
