using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MSDoorLock : MonoBehaviour {

    public GameObject doorLock;
    Rigidbody doorRB;
    float startEulerAnglesY;

    void Start() {
        startEulerAnglesY = this.transform.localEulerAngles.y;
        doorRB = GetComponent<Rigidbody>();
        doorRB.isKinematic = true;
        doorRB.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update() {
        if (doorLock) {
            float distAngle = Mathf.Abs(startEulerAnglesY - transform.localEulerAngles.y);
            float lockZAngle = doorLock.transform.localEulerAngles.z;
            //
            if (lockZAngle > 180 && lockZAngle < 325 && doorRB.isKinematic && distAngle < 2) {
                doorRB.isKinematic = false; //unlock door
                doorRB.constraints = RigidbodyConstraints.None;
            }
            if (lockZAngle > 345 && lockZAngle < 360 && !doorRB.isKinematic && distAngle < 2) {
                doorRB.isKinematic = true; //lock door
                doorRB.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
