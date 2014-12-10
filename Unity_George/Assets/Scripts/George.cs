using UnityEngine;
using System.Collections;

public class George : MonoBehaviour {

	public float PlayerSpeed = 5;
	public GameObject Controller;
	private Controller con;
	public float scaleFactorY=2000f;
    private float oldScale=1;

	// Use this for initialization
	void Start () {
        con = Controller.GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {

		float amtToMove = Input.GetAxisRaw ("Horizontal") * PlayerSpeed;
		transform.Translate(Vector3.right * amtToMove * Time.deltaTime);
        float scale = con.getSmoothSlider1()/scaleFactorY;


        if (scale > oldScale)
        {
            float a = Mathf.Cos(transform.rotation.z * Mathf.Deg2Rad);
            Debug.Log(transform.rotation.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + a*(scale - oldScale) * 0.5f, transform.position.z);
        }

        oldScale = scale;

        if (con.getSmoothSlider1() / scaleFactorY > 0.5)
            transform.localScale = new Vector3(1, scale, 1);


	
	}
}
