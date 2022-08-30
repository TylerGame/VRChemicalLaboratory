using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MSLimitPos : MonoBehaviour {

    Rigidbody myRigidbody;
    Vector3 originalLocalPosition;

    public enum MoveAxis {
        MoveInX, MoveInY, MoveInZ
    };
    public MoveAxis _moveAxis = MoveAxis.MoveInX;
    public float minAxisPos = 0;
    public float maxAxisPos = 1;

	void Awake () {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
        myRigidbody.useGravity = false;
        originalLocalPosition = transform.localPosition;
        //
        if (minAxisPos > maxAxisPos) {
            MSLimitPos thisComponent = this;
            thisComponent.enabled = false;
            Debug.LogWarning("The minimum value cannot be greater than the maximum value.", transform.gameObject);
            return;
        }
    }

    public void MoveObjectInFixedUpdate(Vector3 direction) {
        float lerpSpeed = 0.06f;
        float cross = Vector3.Dot(direction, transform.forward);
        float direc = 1 * Mathf.Sign(cross);
        switch (_moveAxis) {
            case MoveAxis.MoveInX:
                float newAxisPosX = transform.localPosition.x + direc;
                newAxisPosX = Mathf.Clamp(newAxisPosX, minAxisPos, maxAxisPos);
                Vector3 newPosX = new Vector3(newAxisPosX, originalLocalPosition.y, originalLocalPosition.z);
                transform.localPosition = new Vector3(transform.localPosition.x, originalLocalPosition.y, originalLocalPosition.z); //clamp other axis
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPosX, lerpSpeed);
                break;
            case MoveAxis.MoveInY:
                float newAxisPosY = transform.localPosition.y + direc;
                newAxisPosY = Mathf.Clamp(newAxisPosY, minAxisPos, maxAxisPos);
                Vector3 newPosY = new Vector3(originalLocalPosition.x, newAxisPosY, originalLocalPosition.z);
                transform.localPosition = new Vector3(originalLocalPosition.x, transform.localPosition.y, originalLocalPosition.z); //clamp other axis
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPosY, lerpSpeed);
                break;
            case MoveAxis.MoveInZ:
                float newAxisPosZ = transform.localPosition.z + direc;
                newAxisPosY = Mathf.Clamp(newAxisPosZ, minAxisPos, maxAxisPos);
                Vector3 newPosZ = new Vector3(originalLocalPosition.x, originalLocalPosition.y, newAxisPosY);
                transform.localPosition = new Vector3(originalLocalPosition.x, originalLocalPosition.y, transform.localPosition.z); //clamp other axis
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPosZ, lerpSpeed);
                break;
        }
    }
}
