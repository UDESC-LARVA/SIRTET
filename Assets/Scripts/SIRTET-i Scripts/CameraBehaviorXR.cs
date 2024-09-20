using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraBehaviorXR : MonoBehaviour {
	
	BaseController controller;
	
	public int posicaoCamera = 3;
	public int menuScale = 30;
	public int gameScale = 700;

	//public Transform target;
	
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	void Update()	
	{
		
		if(SceneManager.GetActiveScene().name == "Game_Start")
			InGame();
		
		if(SceneManager.GetActiveScene().name == "Menu_Total")
			InMenu();
		
	}
	
	void InGame ()
	{
		
		controller = GameObject.Find("Ambiente").GetComponent<BaseController>();
		
		transform.localPosition = new Vector3(0, 0, -3273);
		transform.localScale = new Vector3(gameScale, gameScale, gameScale);

		Camera[] childCameras = GetComponentsInChildren<Camera>();

        // Loop through each camera and modify it
        foreach (Camera cam in childCameras)
        {
            Debug.Log("Found camera: " + cam.name);

            // Example: Change the culling mask of each child camera
            cam.cullingMask = ~0;
        }
		
	}

	void InMenu ()
	{
		SetLayerRecursively(GameObject.Find("UDP"), "TransparentFX");

		transform.localPosition = new Vector3(0, 0, -300);
		transform.localScale = new Vector3(menuScale, menuScale, menuScale);

		Camera[] childCameras = GetComponentsInChildren<Camera>();

        // Loop through each camera and modify it
        foreach (Camera cam in childCameras)
        {
            Debug.Log("Found camera: " + cam.name);

            // Example: Change the culling mask of each child camera
            cam.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("UI"));

        }
	}

	 public void SetLayerRecursively(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerName);
        }
    }

	/*
	public void Recenter()
	{
		
		if(SceneManager.GetActiveScene().name == "Game_Start")
		{
			transform.localPosition = new Vector3(0, 0, -3273);
		}
		
		if(SceneManager.GetActiveScene().name == "Menu_Total")
		{
			transform.localPosition = new Vector3(0, 0, -300);
		}

		transform.LookAt(target);
		Vector3 currentRotation = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, currentRotation.y, 0);

	}
	*/
}
