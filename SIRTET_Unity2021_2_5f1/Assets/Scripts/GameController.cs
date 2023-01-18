using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	
	public bool showVar = false;
	
	//Global Scripts
	public XMLReader file;
	public Interface gui;
	public UDP kinect;
	public Report report;
	public SoundBehavior sound;
	
	
	//Global Variables between scenes
	public float feedbackVolume = 0.25f;
	
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	void Start()
	{
		SceneManager.LoadScene("Menu_Total");
		
		file = GameObject.Find ("XML").GetComponent<XMLReader> ();
		kinect = GameObject.Find ("UDP").GetComponent<UDP> ();
		sound = GameObject.Find ("Audio").GetComponent<SoundBehavior> ();
		
	}
	
}


