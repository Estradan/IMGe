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
	private bool leftOld=false, rightOld =false;
	private bool rotatedDirection;


	public GameObject Controller;
	public float scaleFactor=819f;
	public float jumpHeight = 10f;
    public Transform spawnPoint;
	[Header("Speeds")]
	public float globalPlayerSpeed = 5;
	public float playerSpeed;
	public float rotationSpeed = 10;
	[Header("Stability")]
	public float stability = 0.3f;
	public float stabilitySpeed = 2.0f;
    [Header("GUI")]
    public GUIStyle Label;

    //private ArrayList objects;
    private bool pause = false;
    private Vector2 velocity = new Vector2();

	// Use this for initialization
	void Start () {
		pause = true;
		if (!controllerPluggedIn){
			verticalScale = transform.localScale.y;
			horizontalScale = transform.localScale.x;
		}else {
			con = Controller.GetComponent<Controller>();

		}
		distToGround = collider2D.bounds.extents.y;
		playerSpeed = globalPlayerSpeed;


        //objects = new ArrayList();
	}
	
	// Update is called once per frame
	void FixedUpdate(){
		if(controllerPluggedIn){
			//Maximalwert Slider = 4095, maximalscale = 5
			verticalScale = con.getSmoothSlider1()/scaleFactor;
			horizontalScale = con.getSmoothSlider2()/scaleFactor;
			
			if(!powerSphere){ //-------------------CUBE---------------------


				//SCALING Beginning
				rotatedDirection = Mathf.Round (Mathf.Abs (Mathf.Sin (transform.eulerAngles.z * Mathf.Deg2Rad) )) == 1;
				if (verticalScale < 0.5f){
					verticalScale = 0.5f;
				} 
				if (horizontalScale < 0.5f){
					horizontalScale = 0.5f;
				}
				if(UpperCollison()&& SideCollision()){
					if(horizontalScale < transform.localScale.x)
						transform.localScale = new Vector3(horizontalScale, transform.localScale.y, 1);
					if(verticalScale < transform.localScale.y)
						transform.localScale = new Vector3(transform.localScale.x, verticalScale, 1);
				}else if(UpperCollison() && SideCollision()){
					if(rotatedDirection){
						if(horizontalScale < transform.localScale.x){
							transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
						}
						else {
							transform.localScale = new Vector3(transform.localScale.x, verticalScale, 1);
						}
					}
					else  {
						if(verticalScale < transform.localScale.y){
							transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
						}
						else {
							transform.localScale = new Vector3(horizontalScale, transform.localScale.y, 1);
						}
					}
				} else if(SideCollision() && !UpperCollison()){
					if(rotatedDirection){
						if(verticalScale < transform.localScale.y){
							transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
						}
						else {
							transform.localScale = new Vector3(horizontalScale, transform.localScale.y, 1);
						}
					}
					else  {
						if(horizontalScale < transform.localScale.x){
							transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
						}
						else {
							transform.localScale = new Vector3(transform.localScale.x, verticalScale, 1);
						}
					}

				} else {
					transform.localScale = new Vector3(horizontalScale,verticalScale,1);
				}
				//SCALING END


				//transform.localScale = new Vector3(horizontalScale,verticalScale,1);


				if(con.getUp() && IsGrounded()){
					rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x,10 / georgeWeight,0);
				}
				
				if(con.getRight() ){
					rightOld = true;
					rigidbody2D.velocity = new Vector2 (playerSpeed, rigidbody2D.velocity.y);
				}
				if(!con.getRight() && rightOld){
					rightOld = false;
					rigidbody2D.velocity = new Vector2 (1, rigidbody2D.velocity.y);
				}
				if(con.getLeft()){
					leftOld = true;
					rigidbody2D.velocity = new Vector2 (-playerSpeed , rigidbody2D.velocity.y);
				}
				if(!con.getLeft() &&leftOld){
					leftOld = false;
					rigidbody2D.velocity = new Vector2 (-1, rigidbody2D.velocity.y);
				}
				
			} else {//-------------------SPHERE---------------------
				if (verticalScale > 0.5f ){
					if(!UpperCollison() && !SideCollision() || verticalScale < transform.localScale.y)
						transform.localScale = new Vector3(verticalScale, verticalScale, 1);
				}
				
				if (con.getRight()){
					rigidbody2D.AddForce(new Vector2(playerSpeed *2, 0));
				}
				if (con.getLeft())
				{
					rigidbody2D.AddForce(new Vector2(-playerSpeed*2, 0));
				}
				
				float rotation = 0;
				//Maximalwert Potis = 4095, 1500-2500 keine rotation, 500/3500 min/max rotation(10);
				float potLeft = con.getSmoothStick1();
				if (potLeft < 500) potLeft = 500;
				if (potLeft > 3500) potLeft = 3500;
				if (potLeft < 1500)
				{
					rotation = (potLeft - 1500) / 100f;
				}
				else if (potLeft > 2500)
				{
					rotation = (potLeft - 2500) / 100f;
				}
				//Debug.Log(rotation);
				if (rotation < -0.001 || rotation > 0.001)
				{
					rigidbody2D.AddTorque(rotation *0.9f );
				}
				
				//Not sure whether we implement jump for the sphere or not
				/*if(con.getUp() && IsGrounded()){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, 10 / georgeWeight,0);
				}*/
			}


		}
		rigidbody2D.AddForce(new Vector2(0,-5));
		distToGround = collider2D.bounds.extents.y;

		}
	void Update () {
        if (!controllerPluggedIn){
            if (Input.GetKey(KeyCode.Escape)){
                pause = true;
                rigidbody2D.isKinematic = true;
            }
        }
        else{
            if (con.getPause()){
                pause = true;
                rigidbody2D.isKinematic = true;
            }
        }
        if (pause) return;

        velocity = rigidbody2D.velocity;

		//Debug.Log (IsGrounded());
		if (powerSphere) {
			rigidbody2D.angularDrag = 2 / (transform.localScale.x * transform.localScale.y);
		} else {
			rigidbody2D.angularDrag = 1;
		}
        rigidbody2D.gravityScale = 0.5f;
		georgeWeight = (0.25f + Mathf.Sqrt(transform.localScale.x * transform.localScale.y)) * 1.5f;
		//float amtToMove = Input.GetAxisRaw ("Horizontal") * playerSpeed;
		//transform.Translate(Vector3.right * amtToMove * Time.deltaTime, Space.World);


		if(!controllerPluggedIn){
			if(!powerSphere){//-------------------CUBE---------------------
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
					rigidbody2D.velocity = new Vector2 (-1, rigidbody2D.velocity.y);
				}
				if (Input.GetKey(KeyCode.P) && transform.localScale.y < 5){
					verticalScale += 2f * Time.deltaTime;
					transform.localScale = new Vector3(horizontalScale,verticalScale,1);
				}
				if (Input.GetKey (KeyCode.O)&& transform.localScale.y > 0.5){
					verticalScale -= 2f * Time.deltaTime;
					transform.localScale = new Vector3(horizontalScale,verticalScale,1);
				}
				if (Input.GetKey (KeyCode.L) && transform.localScale.x < 5){
					horizontalScale +=2f * Time.deltaTime;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKey (KeyCode.K) && transform.localScale.x > 0.5){
					horizontalScale -=2f * Time.deltaTime;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,jumpHeight / georgeWeight);
				}
				if (Input.GetKey(KeyCode.U)){
					//rigidbody2D.velocity = new Vector2 (5, rigidbody2D.velocity.y);
					rigidbody2D.AddTorque(rotationSpeed * Time.deltaTime);
				}

				if (Input.GetKey(KeyCode.J)){
					rigidbody2D.AddTorque(-rotationSpeed * Time.deltaTime);
				}
			}
			else {//-------------------SPHERE---------------------
				if (Input.GetKey(KeyCode.D)){
					rigidbody2D.AddForce(new Vector2(playerSpeed*2,0));
				}

				if (Input.GetKey(KeyCode.A)){
					rigidbody2D.AddForce(new Vector2(-playerSpeed*2,0));

				}

				if (Input.GetKey(KeyCode.P) && transform.localScale.x < 5 && !UpperCollison() && !SideCollision()){
					verticalScale += 2f * Time.deltaTime;
					horizontalScale += 2f * Time.deltaTime;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
				}
				if (Input.GetKey(KeyCode.O) && transform.localScale.x > 0.5){
					verticalScale -= 2f * Time.deltaTime;
					horizontalScale -= 2f * Time.deltaTime;
					transform.localScale = new Vector3(horizontalScale, verticalScale, 1);
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


		if (verticalScale > oldVerticalScale)
        {
            float positionJumping = Mathf.Abs(Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad ));
			transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(verticalScale - oldVerticalScale) * 0.5f, transform.position.z);
			//Debug.Log(positionJumping);
		}
		if(horizontalScale > oldHorizontalScale)
		{
			float positionJumping = Mathf.Abs(Mathf.Sin(transform.localEulerAngles.z * Mathf.Deg2Rad ));
			transform.position = new Vector3(transform.position.x, transform.position.y + positionJumping*(horizontalScale - oldHorizontalScale) * 0.5f, transform.position.z);

		}
	

        oldVerticalScale = verticalScale;
		oldHorizontalScale = horizontalScale;
	
	}

	/*void FixedUpdate () {

		if(!powerSphere){
			/*Vector3 predictedUp = Quaternion.AngleAxis(
				rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stabilitySpeed,
				rigidbody.angularVelocity
				) * transform.up;
		
			Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
			rigidbody.AddTorque(torqueVector * stabilitySpeed * stabilitySpeed);
		}
		//Increase Gravity only for George (Want faster GamePlay)


	}*/

	bool IsGrounded(){
		return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f, 3);
	}
	bool UpperCollison(){
		return Physics2D.Raycast(transform.position, Vector2.up, distToGround + 0.5f, 3);
	}
	bool SideCollision(){
		return Physics2D.Raycast(transform.position, Vector2.right, distToGround + 0.3f, 3) 
			&& Physics2D.Raycast(transform.position, -Vector2.right, distToGround + 0.3f, 3);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "SpherePower" && !powerSphere){
			GameObject sphere = GameObject.FindGameObjectWithTag("SpherePower");
			this.GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
			this.renderer.material=sphere.renderer.material;
			Destroy (this.collider2D);
			horizontalScale = 1; verticalScale = 1;
			transform.localScale = new Vector3(1,1,1);
			gameObject.AddComponent("CircleCollider2D");
			powerSphere = true;
            //other.gameObject.SetActive(false);
            //objects.Add(other.gameObject);
		}
		if(other.gameObject.tag == "CubePower" && powerSphere){
			GameObject square = GameObject.FindGameObjectWithTag("CubePower");
			this.GetComponent<MeshFilter>().mesh = square.GetComponent<MeshFilter>().mesh;
			this.renderer.material=square.renderer.material;
			Destroy (this.collider2D);
			horizontalScale = 1; verticalScale = 1;
			transform.localScale = new Vector3(1,1,1);
			gameObject.AddComponent("BoxCollider2D");
			powerSphere = false;
            //other.gameObject.SetActive(false);
            //objects.Add(other.gameObject);
		}
        if (other.gameObject.tag == "DeathZone"){
            OnDeath();
        }

	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Button") {
			if(transform.localScale.x * transform.localScale.y > 10 && rigidbody2D.velocity.y < -5f){
				other.gameObject.GetComponent<SpringJoint2D>().enabled = false;
				other.gameObject.GetComponent<DistanceJoint2D>().distance = 0f;
				GameObject door = GameObject.FindGameObjectWithTag("Finish");
				door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y +10f, 1);

			}
		}

	}

	float stay = 0.02f;
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "WindBox") {

						rigidbody2D.AddForce (new Vector2 (-15, 0));

						if (!powerSphere) {
								if (playerSpeed > 0 && transform.localScale.x * transform.localScale.y < 10) {
										playerSpeed -= stay;
								} else if (transform.localScale.x * transform.localScale.y > 10) {
										playerSpeed = globalPlayerSpeed;
								}
						} 
						if (powerSphere) {
								//rigidbody2D.AddForce (new Vector2 (-17 , 0));
								if (playerSpeed > 0)
										playerSpeed -= stay;
				
								//rigidbody2D.AddForce (new Vector2 (-40/(windweight) , 0));
								/*rigidbody2D.AddForce (new Vector2 (-5, 0));

						if (playerSpeed > 3)
							playerSpeed -= stay;*/
		
						}
				}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "WindBox")
			playerSpeed = globalPlayerSpeed;
		}

    void OnDeath(){
        //Debug.Log("Dead");
        gameObject.SetActive(false);

        // reset the character's speed
        rigidbody2D.velocity = new Vector2(0,0);

        // reset the character's position to the spawnPoint
        transform.position = spawnPoint.position;

        /*for (int i = 0; i < objects.Count; i++){
            ((GameObject)objects[i]).SetActive(true);
        }
        objects.Clear();*/
        if (!controllerPluggedIn) transform.localScale = new Vector3(1, 1, 1);

        GameObject square = GameObject.FindGameObjectWithTag("CubePower");
        this.GetComponent<MeshFilter>().mesh = square.GetComponent<MeshFilter>().mesh;
        this.renderer.material = square.renderer.material;
        Destroy(this.collider2D);
        horizontalScale = 1; verticalScale = 1;
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.AddComponent("BoxCollider2D");
        powerSphere = false;
        gameObject.SetActive(true);

    }

    void OnGUI()
    {
        if (pause)
        {
            GUI.Box(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 120, 180, 240), "");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 108, 160, 50), "Pause", Label);
            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 50, 160, 50), "Back to Game")){
                pause = false;
                rigidbody2D.isKinematic = false;
                rigidbody2D.velocity = velocity;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2 + 5, 160, 50), "Restart")){
                OnDeath();
                pause = false;
                rigidbody2D.isKinematic = false;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2 + 60, 160, 50), "Exit")){
                Application.Quit();
            }
        }
    }
}