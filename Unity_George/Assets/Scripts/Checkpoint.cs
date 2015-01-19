using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Player")
		{
			GameObject sphere = transform.FindChild("CheckpointSphere").gameObject;
			sphere.light.color = Color.green;
			GameObject.Find("Spawn").gameObject.transform.position = this.transform.position;
			this.collider2D.enabled = false;
		}

	}
}
