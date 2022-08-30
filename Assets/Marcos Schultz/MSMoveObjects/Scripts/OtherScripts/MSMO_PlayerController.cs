using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MSMO_PlayerController : MonoBehaviour {

	GameObject cameraFPS;
	Quaternion originalRotation;
	Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	float rotacaoX = 0.0f, rotacaoY = 0.0f;

	[Header("Player Settings")]
	[Tooltip("The speed at which your player can move. The higher the value of the variable, the faster the player moves.")]
	public float moveSpeed = 6.0f;
	[Tooltip("The strength of the player's leap. The higher the value of the variable, the higher the player can jump.")]
	public float jumpForce = 7.5f;

	[Header("Mouse Sensibility")]
	[Tooltip("The sensitivity of the x-axis")]
	public float sensibilityMouseX = 5.5f;
	[Tooltip("The sensitivity of the y-axis")]
	public float sensibilityMouseY = 5.5f;

	void Start () {
		transform.tag = "Player";
		cameraFPS = GetComponentInChildren (typeof(Camera)).transform.gameObject;
		originalRotation = cameraFPS.transform.localRotation;
		controller = GetComponent<CharacterController>();
	}

	void Update () {
		Vector3 forwardDirection = new Vector3 (cameraFPS.transform.forward.x,0,cameraFPS.transform.forward.z);
		Vector3 sideDirection = new Vector3 (cameraFPS.transform.right.x,0,cameraFPS.transform.right.z);
		forwardDirection.Normalize ();
		sideDirection.Normalize ();
		forwardDirection = forwardDirection * Input.GetAxis ("Vertical");
		sideDirection = sideDirection * Input.GetAxis ("Horizontal");
		Vector3 finalDirection = forwardDirection + sideDirection;
		if (finalDirection.sqrMagnitude > 1) {
			finalDirection.Normalize ();
		}
		if (controller.isGrounded) {
			moveDirection = new Vector3 (finalDirection.x, 0, finalDirection.z);
			moveDirection *= moveSpeed;
			if (Input.GetButton ("Jump")/* && !Input.GetMouseButton(0)*/) {
				moveDirection.y = jumpForce;
			}
		}
		moveDirection.y -= 20.0f * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		FirstPerson ();
	}

	void FirstPerson(){
		if (!MSMoveObjects.rotatingObject) {
			rotacaoX += Input.GetAxis ("Mouse X") * sensibilityMouseX;
			rotacaoY += Input.GetAxis ("Mouse Y") * sensibilityMouseY;
		}
		rotacaoX = ClampAngleFPS (rotacaoX, -360, 360);
		rotacaoY = ClampAngleFPS (rotacaoY, -80, 80);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotacaoX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotacaoY, -Vector3.right);
		Quaternion finalRotation = originalRotation * xQuaternion * yQuaternion;
		cameraFPS.transform.localRotation = Quaternion.Lerp (cameraFPS.transform.localRotation, finalRotation, Time.deltaTime*10.0f);
	}

	float ClampAngleFPS (float angle, float min, float max){
		if (angle < -360F) { angle += 360F; }
		if (angle > 360F) { angle -= 360F; }
		return Mathf.Clamp (angle, min, max);
	}
}