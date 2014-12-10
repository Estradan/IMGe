using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public GameObject George;
	private float playerx,playery,playerz;

	// Use this for initialization
	void Start () {
		playerx = George.transform.position.x;
		playery = George.transform.position.y;
		playerz = George.transform.position.z;

		transform.position = new Vector3 (playerx, playery, playerz -20);

	
	}
	
	// Update is called once per frame
	void Update () {
	
		playerx = George.transform.position.x;
		playery = George.transform.position.y;
		playerz = George.transform.position.z;

		transform.position = new Vector3 (playerx, playery, playerz -20);


	}
}
