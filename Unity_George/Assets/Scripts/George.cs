using UnityEngine;
using System.Collections;

public class George : MonoBehaviour {

	public static bool controllerPluggedIn = false;

	private Controller con;

    private float oldVerticalScale=1, oldHorizontalScale=1;
	private float verticalScale, horizontalScale;
	private float georgeWeight;
	private float distToGround;
	private bool powerSphere;


	public GameObject Controller;
	public float scaleFactor=1024f;
	public float jumpHeight = 10f;
	[Header("Speeds")]
	public float playerSpeed = 5;
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
		georgeWeight = 0.25f + Mathf.Sqrt(transform.localScale.x * transform.localScale.y);
		float amtToMove = Input.GetAxisRaw ("Horizontal") * playerSpeed;
		transform.Translate(Vector3.right * amtToMove * Time.deltaTime, Space.World);
		if(controllerPluggedIn){
            //Maximalwert Slider = 4095, maximalscale = 3.999
			verticalScale = con.getSmoothSlider1()/scaleFactor;
			horizontalScale = con.getSmoothSlider2()/scaleFactor;

			if(!powerSphere){
                if (verticalScale < 0.5f){
                    verticalScale = 0.5f;
                } 
                if (horizontalScale < 0.5f){
                    horizontalScale = 0.5f;
				}
                transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				if(con.getUp() && IsGrounded()){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x,10 / georgeWeight,0);
				}

                Quaternion rotation = new Quaternion(0,0,0,0);
                //Maximalwert Potis = 4095, maximalroation = 360° pro Seite
                float potLeft = con.getSmoothStick1();
                float potRight = con.getSmoothStick2();
                rotation.x = (potLeft + potRight - 4095) / 4095 * 360;
                transform.rotation = rotation;

			} else {
				if (verticalScale > 0.5){
					transform.localScale = new Vector3(verticalScale, verticalScale, 1);
				}
				//Not sure whether we implement jump for the sphere or not
				/*if(con.getUp() && IsGrounded()){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, 10 / georgeWeight,0);
				}*/
			}
		
		}
		else{
			if(!powerSphere){
				if (Input.GetKey(KeyCode.P) && transform.localScale.y < 4){
					verticalScale += 0.1f;
					transform.localScale = new Vector3(horizontalScale,verticalScale,1);
				}
				if (Input.GetKey (KeyCode.O)&& transform.localScale.y > 0.5){
					verticalScale -= 0.1f;
					transform.localScale = new Vector3(horizontalScale,verticalScale,1);
				}
				if (Input.GetKey (KeyCode.L) && transform.localScale.x < 4){
					horizontalScale +=0.1f;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKey (KeyCode.K) && transform.localScale.x > 0.5){
					horizontalScale -=0.1f;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x,jumpHeight / georgeWeight,0);
				}
				if (Input.GetKey(KeyCode.U)){
					rigidbody.AddTorque(0,0,rotationSpeed);
				}
				if (Input.GetKey(KeyCode.J)){
					rigidbody.AddTorque(0,0,-rotationSpeed);
				}
			}
			else {
				if (Input.GetKey(KeyCode.P) && transform.localScale.x < 4){
					verticalScale += 0.1f;
					horizontalScale += 0.1f;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKey(KeyCode.O) && transform.localScale.x > 1){
					verticalScale -= 0.1f;
					horizontalScale -= 0.1f;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKey(KeyCode.U)){
					rigidbody.AddForce(rotationSpeed,0,0);
				}
				if (Input.GetKey(KeyCode.J)){
					rigidbody.AddForce(-rotationSpeed,0,0);
				}
				//Not sure yet whether we want to implement jump for the Sphere or not
				if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x,jumpHeight / georgeWeight,0);
				}

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
		if(!powerSphere){
			Vector3 predictedUp = Quaternion.AngleAxis(
				rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stabilitySpeed,
				rigidbody.angularVelocity
				) * transform.up;
		
			Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
			rigidbody.AddTorque(torqueVector * stabilitySpeed * stabilitySpeed);
		}
		//Increase Gravity only for George (Want faster GamePlay)
		rigidbody.AddForce(0,-5,0);

	}

	bool IsGrounded(){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "SpherePower" && !powerSphere){
			Destroy(other.gameObject);
			GameObject sphere = GameObject.FindGameObjectWithTag("SpherePower");
			this.GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
			Destroy (this.collider);
			gameObject.AddComponent("SphereCollider");
			horizontalScale = 1; verticalScale = 1;
			powerSphere = true;
		}
		if(other.gameObject.tag == "CubePower" && powerSphere){
			Destroy(other.gameObject);
			GameObject square = GameObject.FindGameObjectWithTag("CubePower");
			this.GetComponent<MeshFilter>().mesh = square.GetComponent<MeshFilter>().mesh;
			Destroy (this.collider);
			gameObject.AddComponent("BoxCollider");
			horizontalScale = 1; verticalScale = 1;
			powerSphere = false;
		}
	}
}