using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSLeverController : MonoBehaviour {

    public Transform leverTransform;
    public GameObject lampOn;
    public GameObject lampOff;

    HingeJoint ms_hingeJoint;
    bool isOn = false;

	void Start () {
        ms_hingeJoint = leverTransform.GetComponent<HingeJoint>();
        isOn = false;
        SetLampOnOff(false);

        //set Spring
        if (ms_hingeJoint) {
            JointSpring newSpring = ms_hingeJoint.spring;
            newSpring.spring = 100;
            newSpring.damper = 100;
            newSpring.targetPosition = 30;
            ms_hingeJoint.spring = newSpring;
            ms_hingeJoint.useSpring = true;
        }
    }
	
	void Update () {
        if (leverTransform) {
            if (leverTransform.localEulerAngles.z >= 180 && leverTransform.localEulerAngles.z < 340 && !isOn) {
                isOn = true;
                SetLampOnOff(isOn);
                //
                JointSpring newSpring = ms_hingeJoint.spring;
                newSpring.spring = 100;
                newSpring.damper = 100;
                newSpring.targetPosition = -30;
                ms_hingeJoint.spring = newSpring;
            }

            if (leverTransform.localEulerAngles.z > 20 && leverTransform.localEulerAngles.z < 180 && isOn) {
                isOn = false;
                SetLampOnOff(isOn);
                //
                JointSpring newSpring = ms_hingeJoint.spring;
                newSpring.spring = 100;
                newSpring.damper = 100;
                newSpring.targetPosition = 30;
                ms_hingeJoint.spring = newSpring;
            }
        }
    }

    void SetLampOnOff(bool _lampIsOn) {
        if (lampOn) {
            lampOn.gameObject.SetActive(_lampIsOn);
        }
        if (lampOff) {
            lampOff.gameObject.SetActive(!_lampIsOn);
        }
    }
}
