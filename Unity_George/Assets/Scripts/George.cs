using UnityEngine;
using System.Collections;

public class George : MonoBehaviour {

	public static bool controllerPluggedIn = false;

	private Controller con;

    private float oldVerticalScale=1, oldHorizontalScale=1;
	private float verticalScale, horizontalScale;
	private float georgeWeight;
	private float distToGround;
	private float distToSide;
	private bool powerSphere;
	public LayerMask groundLayer;


	public GameObject Controller;
	public float scaleFactor=1024f;
	public float jumpHeight = 10f;
	[Header("Speeds")]
	public float globalPlayerSpeed = 5;
	public float playerSpeed;
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
		distToGround = collider2D.bounds.extents.y;
		playerSpeed = globalPlayerSpeed;


	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (IsGrounded());


		georgeWeight = 0.25f + Mathf.Sqrt(transform.localScale.x * transform.localScale.y);
		//float amtToMove = Input.GetAxisRaw ("Horizontal") * playerSpeed;
		//transform.Translate(Vector3.right * amtToMove * Time.deltaTime, Space.World);

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
				transform.localScale = new Vector3(horizontalScale, verticalScale, transform.position.z);
				if(con.getUp() && IsGrounded()){
					rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x,10 / georgeWeight,0);
				}

                Quaternion rotation = new Quaternion(0,0,0,0);
                //Maximalwert Potis = 4095, maximalroation = 360° pro Seite
                float potLeft = con.getSmoothStick1();
                float potRight = con.getSmoothStick2();
                rotation.x = (potLeft + potRight - 4095) / 4095 * 360;
                transform.rotation = rotation;

				if(con.getRight()){
					rigidbody2D.velocity = new Vector2 (playerSpeed, rigidbody2D.velocity.y);
				}
				if(con.getRightUp()){
					rigidbody2D.velocity = new Vector2 (1, rigidbody2D.velocity.y);
				}
				if(con.getLeft()){
					rigidbody2D.velocity = new Vector2 (-playerSpeed, rigidbody2D.velocity.y);
				}
				if(con.getLeftUp()){
					rigidbody2D.velocity = new Vector2 (1, rigidbody2D.velocity.y);
				}

			} else {
				if (verticalScale > 0.5){
					transform.localScale = new Vector3(verticalScale, verticalScale, transform.position.z);
				}
				//Not sure whether we implement jump for the sphere or not
				/*if(con.getUp() && IsGrounded()){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, 10 / georgeWeight,0);
				}*/

			}
		
		}
		else{
			if(!powerSphere){
				if (Input.GetKey(KeyCode.D)){
					rigidbody2D.velocity = new Vector2 (playerSpeed, rigidbody2D.velocity.y);
					//rigidbody2D.AddTorque(rotationSpeed);
				}
				if(Input.GetKeyUp(KeyCode.D)){
					rigidbody2D.velocity = new Vector2 (1, rigidbody2D.velocity.y);
				}
				if (Input.GetKey(KeyCode.A)){
					rigidbody2D.velocity = new Vector2 (-playerSpeed, rigidbody2D.velocity.y);
					//rigidbody2D.AddTorque(rotationSpeed);
				}
				if(Input.GetKeyUp(KeyCode.A)){
					rigidbody2D.velocity = new Vector2 (1, rigidbody2D.velocity.y);
				}
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
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,jumpHeight / georgeWeight);
				}
				if (Input.GetKey(KeyCode.U)){
					//rigidbody2D.velocity = new Vector2 (5, rigidbody2D.velocity.y);
					rigidbody2D.AddTorque(rotationSpeed);
				}

				if (Input.GetKey(KeyCode.J)){
					rigidbody2D.AddTorque(-rotationSpeed);
				}
			}
			else {
				if (Input.GetKey(KeyCode.D)){
					rigidbody2D.AddForce(new Vector2(rotationSpeed,0));
				}

				if (Input.GetKey(KeyCode.A)){
					rigidbody2D.AddForce(new Vector2(-rotationSpeed,0));

				}

				if (Input.GetKey(KeyCode.P) && transform.localScale.x < 4){
					verticalScale += 0.1f;
					horizontalScale += 0.1f;
					transform.localScale = new Vector3(horizontalScale, verticalScale, transform.position.z);
				}
				if (Input.GetKey(KeyCode.O) && transform.localScale.x > 1){
					verticalScale -= 0.1f;
					horizontalScale -= 0.1f;
					transform.localScale = new Vector3(horizontalScale, verticalScale, transform.position.z);
				}
				if (Input.GetKey(KeyCode.U)){
					rigidbody2D.AddTorque(-rotationSpeed);
				}
			
				if (Input.GetKey(KeyCode.J)){
					rigidbody2D.AddTorque(rotationSpeed);
				}
				//Not sure yet whether we want to implement jump for the Sphere or not
				if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,jumpHeight / georgeWeight);
				}

			}
		}
		/*if (Physics2D.Raycast (transform.position, -Vector2.right, distToSide + 0.1f, 3))
						rigidbody2D.AddForce(new Vector2 (20, 0));*/


		distToSide = collider2D.bounds.extents.x;
		if (verticalScale > oldVerticalScale)
        {
            float positionJumping = Mathf.Abs(Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad ));
			transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(verticalScale - oldVerticalScale) * 0.5f, transform.position.z);
			Debug.Log(positionJumping);
		}
		if(horizontalScale > oldHorizontalScale)
		{
			float positionJumping = Mathf.Abs(Mathf.Sin(transform.localEulerAngles.z * Mathf.Deg2Rad ));
			transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(horizontalScale - oldHorizontalScale) * 0.5f, transform.position.z);

		}
	

        oldVerticalScale = verticalScale;
		oldHorizontalScale = horizontalScale;
	
	}

	void FixedUpdate () {

		if(!powerSphere){
			/*Vector3 predictedUp = Quaternion.AngleAxis(
				rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stabilitySpeed,
				rigidbody.angularVelocity
				) * transform.up;
		
			Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
			rigidbody.AddTorque(torqueVector * stabilitySpeed * stabilitySpeed);*/
		}
		//Increase Gravity only for George (Want faster GamePlay)
		rigidbody2D.AddForce(new Vector2(0,-5));

	}

	bool IsGrounded(){
		return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f, 3);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "SpherePower" && !powerSphere){
			Destroy(other.gameObject);
			GameObject sphere = GameObject.FindGameObjectWithTag("SpherePower");
			this.GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
			this.renderer.material=sphere.renderer.material;
			Destroy (this.collider2D);
			horizontalScale = 1; verticalScale = 1;
			transform.localScale = new Vector3(1,1,1);
			gameObject.AddComponent("CircleCollider2D");
			powerSphere = true;
		}
		if(other.gameObject.tag == "CubePower" && powerSphere){
			Destroy(other.gameObject);
			GameObject square = GameObject.FindGameObjectWithTag("CubePower");
			this.GetComponent<MeshFilter>().mesh = square.GetComponent<MeshFilter>().mesh;
			this.renderer.material=square.renderer.material;
			Destroy (this.collider2D);
			horizontalScale = 1; verticalScale = 1;
			transform.localScale = new Vector3(1,1,1);
			gameObject.AddComponent("BoxCollider2D");
			powerSphere = false;
		}
	}
	float stay = 0.01f;
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "WindBox")
				if (!powerSphere && transform.localScale.x * transform.localScale.y < 10) {
						rigidbody2D.AddForce (new Vector2 (-10, 0));
						if (playerSpeed > 0)
								playerSpeed -= stay;
				} 
				if (powerSphere) {
						rigidbody2D.AddForce (new Vector2 (-10, 0));
						if (playerSpeed > 0)
								playerSpeed -= stay;
				} else {
					rigidbody2D.AddForce (new Vector2 (-5, 0));
					if (playerSpeed > 3)
						playerSpeed -= stay;
		}
		
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "WindBox")
						playerSpeed = globalPlayerSpeed;
		}

}