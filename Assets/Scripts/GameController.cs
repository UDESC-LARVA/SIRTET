using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	
	public static bool testMode = true;

	public bool showVar = false;

	public bool skipMenu = false;
	
	//Global Scripts
	public XMLReader file;
	public Interface gui;
	public UDP kinect;
	public Report report;
	public SoundBehavior sound;
	
	public bool mr = true;
	
	//Global Variables between scenes
	public float feedbackVolume = 0.1f;
	
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	void Start()
	{
		if (skipMenu)
		{
			SceneManager.LoadScene("Game_Start");
		} 
		else 
		{
			SceneManager.LoadScene("Menu_Total");
		}
		
		file = GameObject.Find ("XML").GetComponent<XMLReader> ();
		kinect = GameObject.Find ("UDP").GetComponent<UDP> ();
		sound = GameObject.Find ("Audio").GetComponent<SoundBehavior> ();
	}
	
}


