using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {
	public GameObject targetA,targetB;
	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    float weight = Mathf.Cos(Time.time * speed * 2f * Mathf.PI) * 0.5f + 0.5f;
	    transform.position = targetA.transform.position * weight
						    + targetB.transform.position * (1-weight);
    
	}
}
