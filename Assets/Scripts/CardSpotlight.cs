using UnityEngine;
using System.Collections;

public class CardSpotlight : MonoBehaviour {

	public Vector3 destinationPoint;
	public float speed = 0.15f;
	private Vector3 velocity = Vector3.zero;

	void Start() {
		destinationPoint = transform.position;
	}

	void Update () {
		transform.position = Vector3.SmoothDamp (transform.position, destinationPoint, ref velocity, speed);

	}	 
}
