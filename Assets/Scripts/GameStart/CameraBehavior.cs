using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraBehavior : MonoBehaviour {
	
	BaseController controller;
	
	public int posicaoCamera = 2;
	
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	void Update()	
	{
		/*if(Application.loadedLevelName == "Game_Start")
			InGame();
		
		if(Application.loadedLevelName == "Menu_Total")
			InMenu();*/
		
		if(SceneManager.GetActiveScene().name == "Game_Start")
			InGame();
		
		if(SceneManager.GetActiveScene().name == "Menu_Total")
			InMenu();
	}
	
	void InGame ()
	{
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MenuInterface>().enabled = false;
		
		controller = GameObject.Find("Ambiente").GetComponent<BaseController>();
		
		switch (posicaoCamera) {
			case 1:
				// Posicao da Camera 1: alto, field of view baixo
				transform.position = new Vector3 (0, (controller.height*5), -(((controller.height*10)*2)+1500));
				GetComponent<Camera>().fieldOfView = 30;
				break;
			case 2:		
				//Posicao da Camera 2: meio, field of view baixo
				transform.position = new Vector3 (0, (controller.height*10)*0.85f, -(((controller.height*10)*2)+1500));
				transform.Rotate(new Vector3(10,0,0));	
				GetComponent<Camera>().fieldOfView = 30;
				break;
			case 3:		
				//Posicao da Camera 3: meio, fild of view alto
				transform.position = new Vector3 (0, (controller.height*5), -(controller.height*10)-1300);
				GetComponent<Camera>().fieldOfView = 60;
				break;
		}
	}

	void InMenu ()
	{
		transform.localPosition = new Vector3(0, 50, -300);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MenuInterface>().enabled = true;
	}
}
