using UnityEngine;
using System.Collections;
[RequireComponent(typeof(LineRenderer))]
public class MSLineRenderCode : MonoBehaviour {

	public Transform point0;
	public Transform target;

	LineRenderer lineComponent;

	void Start () {
		lineComponent = GetComponent<LineRenderer> ();
	}

	void Update () {
		lineComponent.SetPosition (0, point0.position);
		lineComponent.SetPosition (1, target.position);
	}
}
