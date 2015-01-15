using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;

public class Controller : MonoBehaviour {

	static SerialPort stream = new SerialPort("COM3", 115200);

	string receivedDataSlider = "EMPTY";
	string receivedDataButton = "EMPTY";
	//string receivedDataMotor = "EMPTY";
	string receivedDataAcc = "EMPTY";
	string receivedDataBlow = "EMPTY";

	/* Usage:
	 * public GameObject Controller;
	 * private Controller con;
	 * 
	 * And in Start() initialize:
	 * con = Controller.GetComponent<Controller> ();
	 * 
	*/
	private float state1 = 0, state2 = 0, state3 = 0, state4 = 0;
	private bool left=false, up=false, right=false, down=false;
	private bool leftOld=false, upOld=false, rightOld =false, downOld=false;
	private float normX, normY, normZ;
	private float blowvalue;


	// Use this for initialization
	void Start () {
		if(!George.controllerPluggedIn)
			gameObject.SetActive(false);
		stream.Open();
		Debug.Log("Serial Port opened.");
	
	}
	
	// Update is called once per frame
	void Update () {
		stream.Write ("4");
		receivedDataSlider = stream.ReadLine ();
		string[] codeString = receivedDataSlider.Split (' ');
		//Code 1 = top Stick; Code 2 = bottom Stick
		//Code 3 = top Slider; Code 4 = bottom Slider
		float code1 = System.Convert.ToInt32(codeString[1], 16);
		float code3 = System.Convert.ToInt32(codeString[3], 16);
		float code2 = System.Convert.ToInt32(codeString[2], 16);
		float code4 = System.Convert.ToInt32(codeString[4], 16);

		if(state1 == 0 && state3 ==0){ state1 = code1; state3 = code3;}
		state1 = code1*0.1f + state1*0.9f;
		state3 = code3*0.1f + state3*0.9f;

		if(state2 == 0 && state4 ==0){ state2 = code2; state4 = code4;}
		state2 = code2*0.1f + state2*0.9f;
		state4 = code4*0.1f + state4*0.9f;

		//Buttons
		stream.Write("1");
		receivedDataButton = stream.ReadLine();
		int buttonVal;
		//128 links, 256 oben, 64 rechts, 512 unten
		buttonVal = System.Convert.ToInt32(receivedDataButton,16);
		if ((buttonVal & 128) != 0) {
			left = true;
		} else {
			left = false;
		}
		if ((buttonVal & 64) != 0) {
			right = true;
		} else {
			right = false;
		}
		if ((buttonVal & 512) != 0) {
			down = true;
		} else {
			down = false;
		}
		if ((buttonVal & 256) != 0) {
			up = true;
		} else {
			up = false;
		}
	
		stream.Write ("a");
		receivedDataAcc = stream.ReadLine ();
		//Debug.Log (receivedDataAcc);
		string[] dataArr = receivedDataAcc.Split (' ');
		float x = System.Convert.ToInt32 (dataArr [1], 16);
		if (x > 127) {
			x = (x - 256);
		}
		float y = System.Convert.ToInt32 (dataArr [2], 16);
		if (y > 127)
			y = (y - 256);
		float z = System.Convert.ToInt32 (dataArr [3], 16);
		if (z > 127)
			z = (z - 256);
		
		normX = x / 1.5f ;
		normY = y / 1.5f;
		normZ = z / 1.5f;

		stream.Write ("s");
		receivedDataBlow = stream.ReadLine ();
		string[] blowArr = receivedDataBlow.Split (' ');
		//Debug.Log (receivedDataBlow);
		blowvalue = float.Parse(blowArr[1], System.Globalization.CultureInfo.InvariantCulture);


		upOld = up; rightOld = right; leftOld = left; downOld = down;

	}


	public void setRumble (int x){
		stream.Write ("m " + x + "\r\n");
		stream.ReadLine ();
	}

	//0 = green, 1 = red, 2 = yellow, 3 = blue
	public void setLEDon (int x){
		switch (x) {
		case 0:
			stream.Write ("l 0 1\r\n");
			stream.ReadLine ();
			break;
		case 1:
			stream.Write ("l 1 1\r\n");
			stream.ReadLine ();
			break;
		case 2:
			stream.Write ("l 2 1\r\n");
			stream.ReadLine ();
			break;
		case 3:
			stream.Write ("l 3 1\r\n");
			stream.ReadLine ();
			break;
		default:
			break;
		}
	}
	public void LEDclear(){
		stream.Write ("l 1 0\r\n");
		stream.ReadLine ();
		stream.Write ("l 2 0\r\n");
		stream.ReadLine ();
		stream.Write ("l 3 0\r\n");
		stream.ReadLine ();
		stream.Write ("l 0 0\r\n");
		stream.ReadLine ();
	}

	public void LEDAllon(){
		stream.Write ("l 1 1\r\n");
		stream.ReadLine ();
		stream.Write ("l 2 1\r\n");
		stream.ReadLine ();
		stream.Write ("l 3 1\r\n");
		stream.ReadLine ();
		stream.Write ("l 0 1\r\n");
		stream.ReadLine ();
	}

 

	//GETTER
	public string getDataSlider(){
		return receivedDataSlider;
	}
	public int getButton(){
		return System.Convert.ToInt32 (receivedDataButton, 16);
	}
	public float getSmoothStick1(){
		return state1;
	}
	public float getSmoothStick2(){
		return state2;
	}
	public float getSmoothSlider1(){
		return state3;
	}
	public float getSmoothSlider2(){
		return state4;
	}
	public bool getUp(){
		return up;
	}
	public bool getRight(){
		return right;
	}
	public bool getDown(){
		return down;
	}
	public bool getLeft(){
		return left;
	}
	public bool getUpUp(){
		return upOld && !up;
	}
	public bool getRightUp(){
		return rightOld && !right;
	}
	public bool getDownUp(){
		return downOld && !down;
	}
	public bool getLeftUp(){
		return leftOld && !left;
	}

	public float getXAxis(){
		return normX;
	}
	public float getYAxis(){
		return normY;
	}
	public float getZAxis(){
		return normZ;
	}

	public Vector3 getAccelVector3(){
		return new Vector3 (-normX/85.0f, normY/85.0f, -normZ/85.0f);
	}
	public float getBlowvalue(){
		return blowvalue;
	}

}
