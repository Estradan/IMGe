using UnityEngine;
using System.Collections;

public class George : MonoBehaviour {

	public static bool controllerPluggedIn = false;

	private Controller con;

    private float oldVerticalScale=1, oldHorizontalScale=1;
	private float verticalScale, horizontalScale;
	private float georgeWeight;
	private float distToGround;


	public GameObject Controller;
	public float scaleFactor=2000f;
	[Header("Speeds")]
	public float PlayerSpeed = 5;
	public float rotationSpeed = 10;
	[Header("Stability")]
	public float stability = 0.3f;
	public float stabilitySpeed = 2.0f;

	// Use this for initialization
	void Start () {
		if (!controllerPluggedIn){
			verticalScale = transform.localScale.y;
			horizontalScale = transform.localScale.x;
		}else {
			con = Controller.GetComponent<Controller>();

		}
		distToGround = collider.bounds.extents.y;


	}
	
	// Update is called once per frame
	void Update () {
		georgeWeight = transform.localScale.x * transform.localScale.y;
		float amtToMove = Input.GetAxisRaw ("Horizontal") * PlayerSpeed;
		transform.Translate(Vector3.right * amtToMove * Time.deltaTime, Space.World);
		if(controllerPluggedIn){
			verticalScale = con.getSmoothSlider1()/scaleFactor;
			horizontalScale = con.getSmoothSlider2()/scaleFactor;

			if (con.getSmoothSlider1() / scaleFactor > 0.5){
				transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
			}
			if (con.getSmoothSlider2() / scaleFactor > 0.5){
				transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
			}
			if(con.getUp() && IsGrounded()){
				rigidbody.velocity = new Vector3(0,10 / georgeWeight,0);
			}
		
		
		}
		else{
			//Todo: Limit
			if (Input.GetKey(KeyCode.P) && transform.localScale.y < 20){
				verticalScale += 0.1f;
				transform.localScale = new Vector3(horizontalScale,verticalScale,1);
			}
			if (Input.GetKey (KeyCode.O)&& transform.localScale.y > 0.5){
				verticalScale -= 0.1f;
				transform.localScale = new Vector3(horizontalScale,verticalScale,1);
			}
			if (Input.GetKey (KeyCode.L) && transform.localScale.x < 20){
				horizontalScale +=0.1f;
				transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
			}
			if (Input.GetKey (KeyCode.K) && transform.localScale.x > 0.5){
				horizontalScale -=0.1f;
				transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
			}
			if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
				rigidbody.velocity = new Vector3(0,10 / georgeWeight,0);
			}
			if (Input.GetKey(KeyCode.U)){
				rigidbody.AddTorque(0,0,rotationSpeed);
			}
			if (Input.GetKey(KeyCode.J)){
				rigidbody.AddTorque(0,0,-rotationSpeed);
			}
		}
		distToGround = collider.bounds.extents.y;

		if (verticalScale > oldVerticalScale)
        {
            float positionJumping = Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad );
			transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(verticalScale - oldVerticalScale) * 0.5f, transform.position.z);
        }
		if(horizontalScale > oldHorizontalScale)
		{
			float positionJumping = Mathf.Sin(transform.localEulerAngles.z * Mathf.Deg2Rad );
			transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(horizontalScale - oldHorizontalScale) * 0.5f, transform.position.z);
		}
	

        oldVerticalScale = verticalScale;
		oldHorizontalScale = horizontalScale;
	
	}

	void FixedUpdate () {
		Vector3 predictedUp = Quaternion.AngleAxis(
			rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stabilitySpeed,
			rigidbody.angularVelocity
			) * transform.up;
		
		Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
		rigidbody.AddTorque(torqueVector * stabilitySpeed * stabilitySpeed);

	}

	bool IsGrounded(){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

}