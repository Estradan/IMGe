using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	private int ButtonWidth = 200;
	private int ButtonHeight = 50;
	
	public Texture backgroundTexture;
	
	
	void OnGUI()
	{
		
		GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), backgroundTexture);
		
		if (GUI.Button(new Rect(Screen.width/2 - ButtonWidth/2, Screen.height/2 - ButtonHeight/2, ButtonWidth,
			ButtonHeight), "You Won!\n Press here to start again."))
		{

			Application.LoadLevel(0);
			
		}
		
	}
}
