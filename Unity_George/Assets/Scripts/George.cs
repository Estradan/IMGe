using UnityEngine;
using System.Collections;

public class George : MonoBehaviour {

	public static bool controllerPluggedIn = false;


	public float PlayerSpeed = 5;
	public GameObject Controller;
	private Controller con;
	public float scaleFactorY=2000f;
    private float oldScale=1;
	private float scale;

	// Use this for initialization
	void Start () {
		if (!controllerPluggedIn){
			scale = transform.localScale.y;
		}else{
			con = Controller.GetComponent<Controller>();

		}
	}
	
	// Update is called once per frame
	void Update () {

		float amtToMove = Input.GetAxisRaw ("Horizontal") * PlayerSpeed;
		transform.Translate(Vector3.right * amtToMove * Time.deltaTime, Space.World);
		if(controllerPluggedIn){
			scale = con.getSmoothSlider1()/scaleFactorY;

			if (con.getSmoothSlider1() / scaleFactorY > 0.5)
				transform.localScale = new Vector3(1, scale, 1);
			
			}
		
		
		else{
			//Todo: Limit
			if (Input.GetKey(KeyCode.P)){
				scale += 0.1f;
				transform.localScale = new Vector3(1,scale,1);
			}
			if (Input.GetKey (KeyCode.O)){
				scale -= 0.1f;
				transform.localScale = new Vector3(1,scale,1);
			}
		}
		
		if (scale > oldScale)
        {
            float positionJumping = Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad );
            transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(scale - oldScale) * 0.5f, transform.position.z);
        }

        oldScale = scale;
	
	}
}