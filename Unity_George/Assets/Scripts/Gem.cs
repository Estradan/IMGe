using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	
	public GameObject GemChild;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		GemChild.transform.Rotate(Vector3.down * Time.deltaTime * 40, Space.World);
		
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.tag == "Player"){
			//Instantiate(GemPrefab,this.transform.position, Quaternion.identity);
			//Ongui.CrystalCount++;
			Destroy(this.gameObject);
			Application.LoadLevel("End");
		}
	}
}
