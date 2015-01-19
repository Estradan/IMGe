using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public bool sphere;
	private GameObject George;

	// Use this for initialization
	void Start () {
		George = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (George.collider2D.GetType () == typeof(BoxCollider2D)) {
			if (!sphere) {
				this.gameObject.renderer.enabled= false;
				this.gameObject.collider2D.enabled = false;
				this.gameObject.particleSystem.enableEmission = false;
			}
			if (sphere) {
				this.gameObject.renderer.enabled = true;
				this.gameObject.collider2D.enabled = true;
				this.gameObject.particleSystem.enableEmission = true;
			}
		} else {
			if (sphere) {
				this.gameObject.renderer.enabled= false;
				this.gameObject.collider2D.enabled = false;
				this.gameObject.particleSystem.enableEmission = false;
			}
			if (!sphere) {
				this.gameObject.renderer.enabled= true;
				this.gameObject.collider2D.enabled = true;
				this.gameObject.particleSystem.enableEmission = true;
			}

				}

	
	}
}
